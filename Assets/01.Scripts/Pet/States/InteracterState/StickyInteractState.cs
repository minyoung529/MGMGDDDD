using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyInteractState : PetState
{
    public override PetStateName StateName => PetStateName.Interact;

    private float moveSpeed = 1f;

    private StickyPet stickyPet;

    private void Awake()
    {
        stickyPet = transform.parent.GetComponent<StickyPet>();
    }

    public override void OnEnter()
    {
        pet.Event.StartListening((int)PetEventName.OnSetDestination, OnMove);
        ReadySticky();
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnSetDestination, OnMove);
    }

    public override void OnUpdate()
    {
    }

    private void ReadySticky()
    {
        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            transform.DOMove(hit, moveSpeed).OnComplete(()=>
            {
                CheckAroundSticky();
            });
        }
    }

    private void CheckAroundSticky()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider col in cols)
        {
            Sticky stickyObject = col.GetComponent<Sticky>();
            if (stickyObject == null) continue;
            
            if (stickyObject.IsSticky) return;
            stickyPet.StickyObject = stickyObject;
            pet.State.ChangeState((int)PetStateName.Sticky);
            return;
        }
       
    }
    private void OnMove()
    {
        pet.State.ChangeState((int)PetStateName.Move);
    }
}
