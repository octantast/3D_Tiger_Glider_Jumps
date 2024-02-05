using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkingScript : MonoBehaviour
{
    public float blinkSpeed = 1.0f;
    private Color32 thiscolor;
    private TMP_Text text;
    private Color32 whitecolor = new Color32(225, 255, 255, 60);

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        thiscolor = text.color;
    }

    void Update()
    {
        text.color = Color.Lerp(thiscolor, whitecolor, Mathf.PingPong(Time.time, 2f) / 2f);
    }
}
