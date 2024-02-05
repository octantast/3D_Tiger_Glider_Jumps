using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    public GeneralController general;

    private float volume;
    public List<AudioSource> sounds;
    
    private bool reloadThis;
    private bool reload;
    private float loadingtimer = 3;

    public Image loadingScale;

    public GameObject volumeOn;
    public GameObject volumeOff;
    public GameObject loadingScreen;
    public GameObject settingScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    private float mode; // unique level
    public int howManyLevelsDone; // real number of last level
    private int levelMax; // how many levels total
    public float chosenLevel; // real number of level

    // all ui
    public int levelreward;
    public TMP_Text levelcount;
    public TMP_Text levelcoins;
    public int coins;
    public int price1;
    public int price2;
    public TMP_Text price1text;
    public TMP_Text price2text;
    public List<TMP_Text> coinsText;

    public bool a2active;

    private Material lastcolor;
    private int randomIndex;
    private string laststring;

    // tips
    public Animator tipAnimator;
    public Animator tutorialHand;

    public int tutorial1;
    public int tutorial2;
    public int tutorial3;
    public int tutorial4;
    public int tutorial5;
    public int tutorial6;


    public void Start()
    {
        Time.timeScale = 1;
        asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        asyncOperation.allowSceneActivation = false;

        coins = PlayerPrefs.GetInt("coins");
        mode = PlayerPrefs.GetFloat("mode");
        levelMax = PlayerPrefs.GetInt("levelMax");
        volume = PlayerPrefs.GetFloat("volume");
        chosenLevel = PlayerPrefs.GetFloat("chosenLevel");
        howManyLevelsDone = PlayerPrefs.GetInt("howManyLevelsDone");
       
        tutorial1 = PlayerPrefs.GetInt("tutorial1");
        tutorial2 = PlayerPrefs.GetInt("tutorial2");
        tutorial3 = PlayerPrefs.GetInt("tutorial3");
        tutorial4 = PlayerPrefs.GetInt("tutorial4");
        tutorial5 = PlayerPrefs.GetInt("tutorial5");
        tutorial6 = PlayerPrefs.GetInt("tutorial6");

        loadingScale.fillAmount = 0;

        sounds[0].Play();
        if (volume == 1)
        {
            Sound(true);
        }
        else
        {
            Sound(false);
        }

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        settingScreen.SetActive(false);
        loadingScreen.SetActive(false);

        general.arrowa2.SetActive(false);

        tipAnimator.enabled = false;
        price1text.text = price1.ToString("0");
        price2text.text = price2.ToString("0");

        foreach (Image img in general.targetImagesOfLevel)
        {
            img.gameObject.SetActive(false);
        }

        // levels
        if (mode == 0)
        {         
            general.intsforlevel = new List<int> {0, 2};
        }
        else if (mode == 1)
        {
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 1, 3 };
        }
        else if (mode == 2)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 17;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 2, 4, 0 };
        }
        else if (mode == 3)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 15;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 4, 3, 4 };
        }
        else if (mode == 4)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 14;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 2, 0, 4 };
        }
        else if (mode == 5)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 13;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 1, 3, 4, 0 };
        }
        else if (mode == 6)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 12;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 3, 2, 0, 1, 0 };
        }
        else if (mode == 7)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 11;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 2, 1, 0, 4, 3 };
        }
        else if (mode == 8)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 10;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 2, 1, 2, 1 };
        }
        else if (mode == 9)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 9;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 0, 2, 3, 2, 1, 3 };
        }
        else if (mode == 10)
        {
            general.spikesAllowed = true;
            general.howOftenSpikes = 8;
            general.cloudsAllowed = true;
            general.intsforlevel = new List<int> { 3, 0, 4, 1, 3, 2 };
        }

        if (general.spikesAllowed)
        {
            for (int i = 0; i < general.allTilesOnFieldGo.Count; i += general.howOftenSpikes)
            {

                GameObject spikes = Instantiate(general.spikes, transform.position, Quaternion.identity, general.allTilesOnFieldGo[i].transform);
                spikes.transform.localPosition = new Vector3(0, -0.3f, 0.1f);


            }
        }

        general.thisLevelTargetCount = general.intsforlevel.Count;

        for (int i = 0; i < general.thisLevelTargetCount; i++)
        {
            general.targetStringsOfLevel.Add(general.allStrings[general.intsforlevel[i]]);
            general.targetImagesOfLevel[i].color = general.allColors[general.intsforlevel[i]];
        }

        general.currentString = general.targetStringsOfLevel[0];

        levelreward = 100;
        levelcoins.text = "+" + levelreward.ToString("0");
        levelcount.text = "Level " + chosenLevel.ToString("0") + " complete!";

        // + spawn lines
        for (int i = 0; i < general.allTilesOnField.Count; i++)
        {
            randomIndex = Random.Range(2, general.allMaterials.Count);

            general.allTilesOnField[i].material = general.allMaterials[randomIndex];
            general.allTilesOnFieldGo[i].tag = general.allStrings[randomIndex];

            Material objToMove = general.allMaterials[randomIndex];
            general.allMaterials.Remove(objToMove);
            general.allMaterials.Insert(0, objToMove);
            string stringtomove = general.allStrings[randomIndex];
            general.allStrings.Remove(stringtomove);
            general.allStrings.Insert(0, stringtomove);

        }


        general.targetImagesOfLevel[0].gameObject.SetActive(true);

        if (tutorial1 == 0)
        {
            tutorialHand.Play("Tutorial1");
            tipAnimator.Play("Tutor1");
            tipAnimator.enabled = true;
        }
        else if (tutorial5 != 0 && tutorial6 == 0 && coins >= price1)
        {
            general.ui.tutorialHand.gameObject.SetActive(true);
            general.ui.tutorialHand.Play("Tutorial6");
            tipAnimator.Play("Bonuses");
            tipAnimator.enabled = true;
        }
        else
        {
            general.ui.tutorialHand.enabled = false;
            general.ui.tutorialHand.gameObject.SetActive(false);
            tipAnimator.enabled = false;
        }
    }

    public void win()
    {
      
        general.paused = true;
        Debug.Log("win");
        winScreen.SetActive(true);
        if (chosenLevel > howManyLevelsDone)
        {
            PlayerPrefs.SetInt("howManyLevelsDone", (int)chosenLevel);
        }

        coins += levelreward;
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.Save();
    }

    public void lose()
    {

        general.paused = true;
        loseScreen.SetActive(true);

        PlayerPrefs.Save();
    }

    public void Update()
    {
      

        foreach (TMP_Text text in coinsText)
        {
            text.text = coins.ToString("0");
        }

     

        if (loadingScreen.activeSelf == true)
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = 0;
            }

            if (loadingtimer > 0)
            {
                loadingScale.fillAmount += 0.01f;
                   loadingtimer -= Time.deltaTime;
            }
            else
            {
                if (!reload)
                {
                    reload = true;
                    if (reloadThis)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                    else
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }
           }
        }
        if (!loadingScreen.activeSelf)
        {
            foreach (AudioSource audio in sounds)
            {
                audio.volume = volume;
            }
        }
    }

    public void ExitMenu()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        general.paused = false;

        loadingScale.fillAmount = 0;
        loadingScreen.SetActive(true);
    }
    public void reloadScene()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        //general.paused = false;
        reloadThis = true;
        loadingScale.fillAmount = 0;
        loadingScreen.SetActive(true);
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

        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    public void closeIt()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        general.player.blocked = true;
        general.paused = false;
        settingScreen.SetActive(false);
        general.player.enabled = true;
    }

    public void Settings()
    {
        Time.timeScale = 0;
        general.player.enabled = false;
        sounds[1].Play();
        general.player.blocked = true;
        general.paused = true;
        settingScreen.SetActive(true);
    }

    public void tutorialcheck6()
    {
        if (general.ui.tutorial6 == 0)
        {
            general.ui.tutorialHand.enabled = false;
            general.ui.tutorialHand.gameObject.SetActive(false);
            general.ui.tutorial6 = 1;
            PlayerPrefs.SetInt("tutorial6", 1);
            PlayerPrefs.Save();
            general.ui.tipAnimator.gameObject.SetActive(false);
            general.ui.tipAnimator.enabled = false;
        }
    }
    public void a1()
    {
        general.player.blocked = true;
        sounds[1].Play();
        if (coins >= price1)
        {
            if (!general.shield.activeSelf)
            {
                tutorialcheck6();
                sounds[4].Play();
                coins -= price1;
                PlayerPrefs.SetInt("coins", coins);
                PlayerPrefs.Save();
                general.shield.SetActive(true);
            }
        }
        else
        {
            general.ui.tipAnimator.gameObject.SetActive(true);
            tipAnimator.enabled = true;
            tipAnimator.Play("Warning", 0, 0);

        }

    }

    public void a2()
    {
        general.player.blocked = true;
        sounds[1].Play();

        if (coins >= price2)
        {
            if (!a2active)
            {
                tutorialcheck6();
                general.a2activated();
                coins -= price2;
                PlayerPrefs.SetInt("coins", coins);
                PlayerPrefs.Save();
            }
        }
        else
        {
            general.ui.tipAnimator.gameObject.SetActive(true);
            tipAnimator.enabled = true;
            tipAnimator.Play("Warning", 0, 0);

        }
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        sounds[1].Play();
        if (chosenLevel <= howManyLevelsDone + 1 && chosenLevel != levelMax)
        {
            chosenLevel += 1;
            mode += 1;
            if (mode > 10)
            {
                mode = 1;
            }

            PlayerPrefs.SetFloat("chosenLevel", chosenLevel);
            PlayerPrefs.SetFloat("mode", mode);
            PlayerPrefs.Save();
            reloadScene();
        }
    }
}
