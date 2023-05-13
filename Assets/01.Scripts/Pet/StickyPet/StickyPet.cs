using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StickyPet : Pet
{
    private void Start()
    {
        InteractAction = InteractEvent;
    }
    public override void ResetPet()
    {
        base.ResetPet();

    }
  
    private void InteractEvent()
    {
        State.ChangeState((int)PetStateName.Sticky);
    }
}