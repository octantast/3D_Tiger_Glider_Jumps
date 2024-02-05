using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Size : MonoBehaviour
{
    private Vector3 startSize;
    void Start()
    {
        startSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, 1); //4* Time.deltaTime);
    }
}
