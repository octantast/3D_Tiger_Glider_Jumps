using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public GeneralController general;
    private Vector3 targetPos;
    public float speed;
    void Start()
    {
        Debug.Log("Cloud spawned");
        speed = Random.Range(0.002f, 0.06f);
           targetPos = new Vector3(transform.localPosition.x*-1, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (general != null && !general.paused)
        {
            targetPos.y = general.player.transform.position.y;
            targetPos.y = Mathf.Clamp(targetPos.y,3f, 50);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, targetPos.y, transform.localPosition.z);
            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            general.player.CloudCollision(this.gameObject, other.ClosestPointOnBounds(transform.position));
        }
    }
}
