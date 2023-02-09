using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : TorchLight
{
    [SerializeField] int index;

    public void Lighting()
    {
        if (IsOn) OffLight();
        else OnLight();
    }

    protected override void FireCollision()
    {
        base.FireCollision();
        
        TorchManager.Instance.LightOn(index);
    }
}
