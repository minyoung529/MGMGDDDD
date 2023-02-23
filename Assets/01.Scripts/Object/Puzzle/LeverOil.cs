using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverOil : Lever
{
    private InteractOilObject interactOil;

    private void Start()
    {
        interactOil = GetComponent<InteractOilObject>();
    }

    protected override bool CheckLever()
    {
        return NearPlayer() && (!interactOil || (interactOil && !interactOil.IsRust));
    }

}
