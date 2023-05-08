using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PetState {
    public override PetStateName StateName => PetStateName.Idle;

    public override void OnEnter() {
        Debug.Log("IdleEnter");
        pet.Event.StartListening((int)PetEventName.OnMoveToDestination, OnSetDestination);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StartListening((int)PetEventName.OnSkillKeyPress, OnSKill);
        pet.Event.StartListening((int)PetEventName.OnHold, OnHold);
    }

    public override void OnExit() {
        pet.Event.StopListening((int)PetEventName.OnMoveToDestination, OnSetDestination);
        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StopListening((int)PetEventName.OnSkillKeyPress, OnSKill);
        pet.Event.StopListening((int)PetEventName.OnHold, OnHold);
    }

    public override void OnUpdate() {
        //이후 Idle 애니메이션을 실행하거나 하는 등 추가가 될 것


        CheckArrive();
        FollowTarget();
    }



    private void CheckArrive()
    {
        if (Vector3.Distance(pet.Agent.destination, transform.position) <= 1f)
        {
            pet.OnArrive?.Invoke();
            pet.OnArrive = null;
        }
    }
    private void FollowTarget()
    {
        if (!pet.Target || !pet.Agent.enabled || !pet.Agent.isOnNavMesh) return;
        pet.Agent.SetDestination(pet.Target.position);
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