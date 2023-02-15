using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorchPuzzle : TorchLight
{
    [SerializeField] int index;
    [SerializeField] private ParticleSystem shortParticle;

    [SerializeField]
    private bool puzzleTorch = true;

    [SerializeField]
    private UnityEvent OnLighting;

    private TorchManager torchM;

    private void Awake()
    {
        torchM = GetComponentInParent<TorchManager>();
    }
    public void Lighting()
    {
        if (IsOn) OffLight();
        else OnLight();
    }

    protected override void FireCollision()
    {
        base.FireCollision();

        shortParticle.Play();

        if (puzzleTorch)
        {
            torchM.LightOn(index);
        }
        else
        {
            OnLighting?.Invoke();
        }
    }
}
