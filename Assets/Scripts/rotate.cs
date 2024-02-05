using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0f, 400f, 0), Time.deltaTime * speed);
    }
}
