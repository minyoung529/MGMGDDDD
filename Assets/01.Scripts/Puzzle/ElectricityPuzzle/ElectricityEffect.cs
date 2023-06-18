using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem electricityParticle;

    [SerializeField]
    private ParticleSystem failParticle;

    public void Play()
    {
        failParticle.Stop();
        electricityParticle.Play();
    }

    public void Fail()
    {
        electricityParticle.Stop();
        failParticle.Play();
    }

    public void Success()
    {
        failParticle.Stop();
        electricityParticle.Stop();
    }
}
