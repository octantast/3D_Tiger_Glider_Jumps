using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public Image loading;
    public float timerforloading = 7.5f;
    private float timerforloadingMax;
    private void Start()
    {
        timerforloadingMax = timerforloading;
    }

    private void Update()
    {
        if (timerforloading > 0)
        {
            timerforloading -= Time.deltaTime;
            loading.fillAmount = 1 - timerforloading/ timerforloadingMax;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
