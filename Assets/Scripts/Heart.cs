using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Heart : MonoBehaviour
{
    private Image thisImage;
    public Color32 targetColor;
    public float fadeduration;
    void Start()
    {
        thisImage = transform.GetComponent<Image>();
    }
    private void Update()
    {
        if (thisImage != null)
        {
            thisImage.color = Color.Lerp(thisImage.color, targetColor, fadeduration * Time.deltaTime);
        }
    }
}
