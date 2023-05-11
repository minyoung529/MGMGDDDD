using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractState : PetState
{
    public override PetStateName StateName => PetStateName.Interact;
    private float radius = 2f;

    public Action ac;

    public override void OnEnter()
    {
        MoveArrived();
        pet.Event.StopListening((int)PetEventName.OnArrive, CheckAroundInteract);
        pet.Event.StartListening((int)PetEventName.OnArrive, CheckAroundInteract);
    }

    public override void OnExit()
    {
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
            pet.State.ChangeState((int)PetStateName.Move);
        }
    }


    private void CheckAroundInteract()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in cols)
        {
            OutlineScript interact = col.GetComponent<OutlineScript>();
            if (interact == null) continue;

            if (pet.GetPetType == PetType.StickyPet)
                interact.OnInteract(() => pet.State.ChangeState((int)PetStateName.Sticky));
            else
                interact.OnInteract();
            break;
        }
        pet.State.ChangeState((int)PetStateName.Idle);
        pet.Event.StopListening((int)PetEventName.OnArrive, CheckAroundInteract);
    }

}
