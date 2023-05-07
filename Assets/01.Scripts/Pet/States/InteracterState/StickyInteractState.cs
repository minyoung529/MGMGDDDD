using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyInteractState : PetState
{
    public override PetStateName StateName => PetStateName.Interact;

    private float moveSpeed = 1f;

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }


    private void ReadySticky()
    {
        Vector3 hit = GameManager.Instance.GetCameraHit();
        if (hit != Vector3.zero)
        {
            transform.DOMove(hit, moveSpeed);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Sticky stickyObject = collision.collider.GetComponent<Sticky>();
        if (stickyObject != null)
        {
            pet.State.ChangeState((int)PetStateName.Sticky);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Sticky stickyObject = other.GetComponent<Sticky>();
        if (stickyObject != null)
        {
            pet.State.ChangeState((int)PetStateName.Sticky);
        }
    }


}
