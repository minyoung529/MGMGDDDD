using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    ParticleSystem fireParticle;

    private void Awake()
    {
        fireParticle = transform.GetChild(1).GetComponent<ParticleSystem>();

        OffLight();
    }

    public void OnLight()
    {
        fireParticle.Play();
    }

    public void OffLight()
    {
        fireParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        FireBall fire = other.GetComponent<FireBall>();
        if(fire !=null)
        {
            Destroy(fire.gameObject);
            OnLight();
        }
    }
}
