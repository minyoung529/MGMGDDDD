using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldState : PetState {
    public override PetStateName StateName => PetStateName.Held;

    public override void OnEnter() {
        Enable();
        pet.Event.StartListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StartListening((int)PetEventName.OnDrop, OnDrop);
    }

    public override void OnExit() {
        Disable();

        pet.Event.StopListening((int)PetEventName.OnThrew, OnThrew);
        pet.Event.StopListening((int)PetEventName.OnDrop, OnDrop);
    }

    public override void OnUpdate() {

    }

    private void Enable() {
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = true;
        pet.Coll.enabled = false;
        pet.SetNavEnabled(false);
    }

    private void Disable() {
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = false;
    }

    private void OnThrew() {
        pet.State.ChangeState((int)PetStateName.Threw);
    }

    private void OnDrop() {
        Debug.Log(1);
        pet.Coll.enabled = true;
        pet.Rigid.isKinematic = false;
        pet.Rigid.velocity = Vector3.zero;
        pet.SetNavEnabled(true);
        pet.State.ChangeState((int)PetStateName.Idle);
    }
}
