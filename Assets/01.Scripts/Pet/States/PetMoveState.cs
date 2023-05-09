using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMoveState : PetState {
    public override PetStateName StateName => PetStateName.Move;

    public override void OnEnter() {
        pet.Agent.SetDestination(pet.destination);
        pet.Event.StartListening((int)PetEventName.OnSetDestination, OnSetDestination);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StartListening((int)PetEventName.OnSkillKeyPress, OnSKill);
        pet.Event.StartListening((int)PetEventName.OnHold, OnHold);
    }

    public override void OnExit() {
        pet.Event.StopListening((int)PetEventName.OnSetDestination, OnSetDestination);
        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StopListening((int)PetEventName.OnSkillKeyPress, OnSKill);
        pet.Event.StopListening((int)PetEventName.OnHold, OnHold);
    }

    public override void OnUpdate() {
        CheckArrive();
    }

    private void CheckArrive() {
        if (Vector3.Distance(pet.destination, transform.position) <= 1f) {
            pet.Event.TriggerEvent((int)PetEventName.OnArrive);
            pet.State.ChangeState((int)PetStateName.Idle);
        }
        else if (Vector3.Distance(pet.GetDestination(), transform.position) <= 1f) {
            pet.Event.TriggerEvent((int)PetEventName.OnStop);
            pet.State.ChangeState((int)PetStateName.Idle);
        }
    }
    private void OnSetDestination() {
        pet.State.ChangeState((int)PetStateName.Move);
    }

    private void OnRecall() {
        pet.State.ChangeState((int)PetStateName.Recall);
    }

    private void OnSKill() {
        pet.State.ChangeState((int)PetStateName.Skill);
    }

    private void OnHold() {
        pet.State.ChangeState((int)PetStateName.Held);
    }
}
