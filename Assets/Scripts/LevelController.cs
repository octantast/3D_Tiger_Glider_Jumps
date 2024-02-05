using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private float initialLaunch;
    private float loadingtimer = 3;

    public int howManyLevelsDone;
    public float chosenLevel;
    public int levelMax;

    public Image loading;

    //public Image levelIndicator;
   // public TMP_Text levelCounter;

    public Color32 notenableButton;
    public Color32 enableButton;

    public List<Image> buttons; // levels
    public List<ButtonScript> buttonscripts;
    public GameObject leftarrow;
    public GameObject righarrow;

    public GameObject startButtons;
    public GameObject levelButtons;
    public GameObject loadingScreen;
    public GameObject settings;

    // music
    private float volume;
    public AudioSource ambient;
    public AudioSource tapSound;
    public GameObject volumeOn;
    public GameObject volumeOff;

    // currency
    //public TMP_Text currencyCount;
    // private int coins;


    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }
    void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        initialLaunch = PlayerPrefs.GetFloat("initialLaunch");
        if (initialLaunch == 0)
        {
            PlayerPrefs.SetFloat("initialLaunch", 1);
            volume = 1;
            PlayerPrefs.SetFloat("volume", volume);
            PlayerPrefs.Save();
        }
        else
        {
            volume = PlayerPrefs.GetFloat("volume");
        }

       

        ambient.Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

      
       // coins = PlayerPrefs.GetInt("coins");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");

        //levelIndicator.fillAmount = howManyLevelsDone / levelMax;
        //levelCounter.text = howManyLevelsDone.ToString("0") + "/" + levelMax.ToString("0");

        for (int i = 0; i <= howManyLevelsDone + 1; i++)
        {
            if (i < buttons.Count)
            {
                buttonscripts[i].thisImage.color = enableButton;
            }
        }

       // currencyCount.text = coins.ToString("0");


        settings.SetActive(false);
        loadingScreen.SetActive(false);
        startButtons.SetActive(true);
        levelButtons.SetActive(false);

        asyncOperation.allowSceneActivation = false;


    }
   
 
    private void Update()
    {

        if (loadingScreen.activeSelf == true)
        {
            loading.fillAmount += 0.01f;
            ambient.volume -= 0.1f;
            tapSound.volume -= 0.1f;
            if (loadingtimer > 0)
            {
                loadingtimer -= Time.deltaTime;
            }
            else
            {
                asyncOperation.allowSceneActivation = true;
            }
        }

    }
    public void StartGame(float mode)
    {
        // cycled
        playSound(tapSound);
        if (mode <= howManyLevelsDone + 1)
        {
            PlayerPrefs.SetInt("levelMax", levelMax);

            PlayerPrefs.SetFloat("chosenLevel", mode);

            // cycled
            for (int j = 1; j <= 10; j++)
            {
                for (int i = j; i <= levelMax; i += 10) // if 11, j = 1
                {
                    if (mode == i)
                    {
                        mode = j;
                    }
                    if (mode == 0)
                    {
                        mode = 1;
                    }
                }
            }

            // unique levels

            loadingScreen.SetActive(true);
            levelButtons.SetActive(false);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
        }
    }

    public void ShowLevels()
    {
        playSound(tapSound);
        loadingScreen.SetActive(false);
        startButtons.SetActive(false);
        levelButtons.SetActive(true);

        leftarrow.SetActive(false);
        righarrow.SetActive(true);

        foreach (ButtonScript button in buttonscripts)
        {
            button.thisLevelNumber = buttonscripts.IndexOf(button) + 1;
            buttonCheck(button);
        }
    }
    public void HideLevels()
    {
        startButtons.SetActive(true);
        playSound(tapSound);
        settings.SetActive(false);
        loadingScreen.SetActive(false);
        levelButtons.SetActive(false);
    }
    public void Settings()
    {
        playSound(tapSound);
        if (!settings.activeSelf)
        {
            HideLevels();
            settings.SetActive(true);
        }
        else
        {
            HideLevels();
            settings.SetActive(false);
        }
    }
    public void Sound(bool volumeBool)
    {
        if (volumeBool)
        {
            volumeOn.SetActive(true);
            volumeOff.SetActive(false);
            volume = 1;

        }
        else
        {
            volume = 0;
            volumeOn.SetActive(false);
            volumeOff.SetActive(true);
        }
        ambient.volume = volume;
        tapSound.volume = volume;

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }
    public void playSound(AudioSource sound)
    {
        sound.Play();
    }



    public void leftArrow()
    {
        playSound(tapSound);
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber -= 12;
            buttonCheck(button);
        }
        if (buttonscripts[0].thisLevelNumber <= 1)
        {
            leftarrow.SetActive(false);
        }
        righarrow.SetActive(true);

    }

    public void rightArrow()
    {
        playSound(tapSound);
        foreach (ButtonScript button in buttonscripts)
        {
            // changes text
            button.thisLevelNumber += 12;
            buttonCheck(button);
            if (button.thisLevelNumber > levelMax)
            {
                button.gameObject.SetActive(false);
                righarrow.SetActive(false);
            }

        }
        leftarrow.SetActive(true);

    }

    public void buttonCheck(ButtonScript button)
    {
        button.thisLevelNumberText.text = button.thisLevelNumber.ToString("0");
        button.gameObject.SetActive(true);
        if (button.thisLevelNumber <= howManyLevelsDone + 1)
        {
            button.thisImage.color = enableButton;
        }
        else
        {
            button.thisImage.color = notenableButton;
        }
    }
}
