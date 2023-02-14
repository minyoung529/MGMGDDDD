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
            TorchManager.Instance.LightOn(index);
        }
        else
        {
            OnLighting?.Invoke();
        }
    }
}
