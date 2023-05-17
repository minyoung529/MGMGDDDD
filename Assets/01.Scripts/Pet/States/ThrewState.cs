using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrewState : PetState {
    public override PetStateName StateName => PetStateName.Threw;

    public override void OnEnter() {
        pet.Rigid.constraints = RigidbodyConstraints.None;
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = false;
        pet.Coll.enabled = true;

        pet.Event.StartListening((int)PetEventName.OnLanding, OnLanding);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnExit() {
        pet.Event.StopListening((int)PetEventName.OnLanding, OnLanding);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnUpdate() {
        if(pet.transform.position.y <= -100f) {
            pet.Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);
        }
    }

    private void OnLanding() {
        pet.State.ChangeState((int)PetStateName.Landing);
    }

    private void OnRecall() {
        pet.State.ChangeState((int)PetStateName.Recall);
    }
}

