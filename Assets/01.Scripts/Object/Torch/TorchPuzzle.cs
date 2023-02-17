using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorchPuzzle : TorchLight
{
    [SerializeField] int index;
    [SerializeField] private ParticleSystem shortParticle;

    private TorchManager torchM;

    protected override void Awake()
    {
        base.Awake();
        torchM = GetComponentInParent<TorchManager>();
    }
    

    public override void FireCollision()
    {
        shortParticle.Play();
        torchM.LightOn(index);
    }
}
