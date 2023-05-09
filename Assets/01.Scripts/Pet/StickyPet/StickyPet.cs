using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StickyPet : Pet
{

    protected override void ResetPet()
    {
        base.ResetPet();

        IsMovePointLock = false;
    }
}