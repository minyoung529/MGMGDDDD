using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldState : PetState
{
    public override PetStateName StateName => PetStateName.Held;

    public override void OnEnter()
    {
        pet.OnHold();
        pet.Event.StartListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StartListening((int)PetEventName.OnDrop, OnDrop);
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StopListening((int)PetEventName.OnDrop, OnDrop);
    }

    public override void OnUpdate()
    {
        Vector3 forward = GameManager.Instance.PlayerController.transform.forward;
        forward.y = 0f;
        pet.transform.forward = forward;
    }

    private void OnThrew()
    {
        pet.State.ChangeState((int)PetStateName.Threw);
    }

    private void OnDrop()
    {
        pet.OnDrop();
        pet.State.ChangeState((int)PetStateName.Idle);
    }
}