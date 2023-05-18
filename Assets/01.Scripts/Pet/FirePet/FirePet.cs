using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePet : Pet
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
        State.ChangeState((int)PetStateName.Idle);
    }

    private void OnDisable()
    {
        Event.StopListening((int)PetEventName.OnInteractEnd, InteractEvent);
    }
}
