using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDropperPattern : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] fireParticle;

    [ContextMenu("Play Particle")]
    public void PlayParticlesAtAll()
    {
        foreach(var particle in fireParticle)
        {
            particle.Play();
        }
    }
}
