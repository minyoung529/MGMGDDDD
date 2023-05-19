using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StickyPet : Pet
{
    private void Start()
    {
        Event.StartListening((int)PetEventName.OnInteractEnd, InteractEvent);
    }

    public override void ResetPet()
    {
        base.ResetPet();
    }
  
    private void InteractEvent()
    {
        if(SelectedObject.CurInteractObject == null)return;
        State.ChangeState((int)PetStateName.Sticky);
    }

    private void OnDisable()
    {
        Event.StopListening((int)PetEventName.OnInteractEnd, InteractEvent);
    }
}