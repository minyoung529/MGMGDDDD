using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldState : PetState {
    public override PetStateName StateName => PetStateName.Held;

    public override void OnEnter() {
        Debug.Log("HoldEnter");
        Enable();
        pet.Event.StartListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StartListening((int)PetEventName.OnHold, OnDrop);
    }

    public override void OnExit() {
        Disable();

        pet.Event.StopListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StopListening((int)PetEventName.OnHold, OnDrop);
    }

    public override void OnUpdate() {

    }

    private void Enable() {
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = true;
        pet.IsInputLock = true;
        pet.Coll.enabled = false;
        pet.SetNavEnabled(false);
    }

    private void Disable() {
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = false;
        pet.IsInputLock = false;
    }

    private void OnThrew() {
        pet.State.ChangeState((int)PetStateName.Threw);
    }

    private void OnDrop() {
        pet.Coll.enabled = true;
        pet.SetNavEnabled(true);
        pet.State.ChangeState((int)PetStateName.Idle);
    }
}
