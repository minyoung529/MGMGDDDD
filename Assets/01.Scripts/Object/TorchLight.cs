using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    ParticleSystem fireParticle;

    private void Awake()
    {
        fireParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void OnLight()
    {
        fireParticle.Play();
    }

    public void OffLight()
    {
        fireParticle.Stop();
    }
}
