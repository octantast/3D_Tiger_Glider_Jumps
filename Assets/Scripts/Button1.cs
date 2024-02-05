using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Button1 : MonoBehaviour//, IBeginDragHandler, IDragHandler, IEndDragHandler, 
{
    public float startSize;
    public Color32 startColor;

    public TMP_Text textObj;

    void Start()
    {
        textObj = transform.GetComponent<TMP_Text>();
        startSize = transform.localScale.x;
        startColor = textObj.color;
    }
    private void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(startSize, startSize, startSize), 10 * Time.deltaTime);
        textObj.color = Color.Lerp(textObj.color, startColor, 10 * Time.deltaTime);
    }
    public void OnEnter()
    {
        textObj.color = new Color32(255, 255, 255, 130);
        transform.localScale = new Vector3(startSize + 0.1f, startSize + 0.1f, startSize + 0.1f);
    }

  
}
