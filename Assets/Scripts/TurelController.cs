using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurelController : MonoBehaviour
{
    public GeneralController general;
    public Rigidbody rb;
    public float torqueImpulse;
    public float currentImpulse;

    private Vector3 startPosition;

    private ParticleSystem newParticleSystem;
    void Start()
    {
        currentImpulse = torqueImpulse;
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(transform.forward * torqueImpulse, ForceMode.Impulse);
        startPosition = transform.localPosition;
    }

    void Update()
    {
       
        transform.localPosition = startPosition;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (newParticleSystem == null)
            {
                ParticleSystem newParticleSystem = Instantiate(general.effects[0], collision.contacts[0].point, Quaternion.identity);
            }
            else
            {
                newParticleSystem.transform.position = collision.contacts[0].point;
                newParticleSystem.Play();
            }
           

            Debug.Log(collision.gameObject.tag);
            respin();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            if (newParticleSystem == null)
            {
                newParticleSystem = Instantiate(general.effects[0], other.ClosestPointOnBounds(transform.position), Quaternion.identity);
            }
            else
            {
                newParticleSystem.transform.position = other.ClosestPointOnBounds(transform.position);
                newParticleSystem.Play();
            }
            general.ui.sounds[4].Play();
            Debug.Log(other.gameObject.tag);
            respin();
        }
    }

    public void respin()
    {
        currentImpulse = currentImpulse * -1;
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(transform.forward * currentImpulse, ForceMode.Impulse);
    }
}
