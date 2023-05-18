using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractState : PetState
{
    public override PetStateName StateName => PetStateName.Interact;
    private float radius = 5f;

    public override void OnEnter()
    {
        pet.Event.StopListening((int)PetEventName.OnArrive, CheckAroundInteract);
        pet.Event.StartListening((int)PetEventName.OnArrive, CheckAroundInteract);
        pet.Event.StartListening((int)PetEventName.OnSetDestination, OnSetDestinationMove);
        pet.Event.StartListening((int)PetEventName.OnActiveInteract, Active);

        MoveArrived();
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnActiveInteract, Active);
        pet.Event.StopListening((int)PetEventName.OnSetDestination, OnSetDestinationMove);
    }

    public override void OnUpdate()
    {
    }

    private void MoveArrived()
    {
        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            pet.SetDestination(hit);
        }
    }

    private void Active()
    {
        pet.Event.TriggerEvent((int)PetEventName.OnInteractEnd);
    }

    private void OnSetDestinationMove()
    {
        pet.State.ChangeState((int)PetStateName.Move);
    }

    private void CheckAroundInteract()
    {
        pet.Event.StopListening((int)PetEventName.OnArrive, CheckAroundInteract);

        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in cols)
        {
            OutlineScript interact = col.GetComponent<OutlineScript>();
            if (interact == null) continue;

            interact.OnInteract();
            break;
        }
    }

}
