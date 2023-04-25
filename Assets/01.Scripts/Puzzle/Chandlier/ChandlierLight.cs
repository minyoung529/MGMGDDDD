using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandlierLight : MonoBehaviour
{
    private ChangeEmission emission;
    private ParticleSystem[] particles;

    private void Awake()
    {
        emission = GetComponent<ChangeEmission>();
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void Lighting()
    {
        emission.Change();

        foreach(var particle in particles)
        {
            particle.Play();
        }
    }
}
