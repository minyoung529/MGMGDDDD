using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : TorchLight
{
    [SerializeField] int index;
    [SerializeField] private ParticleSystem shortParticle;
    
    public void Lighting()
    {
        if (IsOn) OffLight();
        else OnLight();
    }

    protected override void FireCollision()
    {
        base.FireCollision();
        
        shortParticle.Play();
        TorchManager.Instance.LightOn(index);
    }
}
