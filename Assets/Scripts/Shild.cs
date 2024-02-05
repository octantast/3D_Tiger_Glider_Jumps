using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour
{
    public float scaleChangeSpeed = 1.0f;
    public Vector3 startScale;
    public Vector3 targetScale;
    public float rotationSpeed = 45.0f;

    private void Start()
    {

        startScale = transform.localScale;
        targetScale = startScale * 1.5f;
        gameObject.SetActive(false);
    }
    void Update()
    {
        float t = Mathf.PingPong(Time.deltaTime * scaleChangeSpeed, 1.0f);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);

        float rotationAngle = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAngle);
    }
}
