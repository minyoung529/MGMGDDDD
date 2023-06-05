using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StickyPet : Pet
{
    StickyState stickyInteract;

    private void Start()
    {
        stickyInteract = GetComponentInChildren<StickyState>();
        Event.StartListening((int)PetEventName.OnInteractArrive, InteractEvent);
    }

    public override void ResetPet()
    {
        base.ResetPet();
    }
  
    private void InteractEvent()
    {
        stickyInteract.OnSticky();
    }

    private void OnDisable()
    {
        Event.StopListening((int)PetEventName.OnInteractArrive, InteractEvent);
    }
}