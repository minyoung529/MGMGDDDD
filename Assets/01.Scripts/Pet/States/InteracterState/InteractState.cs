using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : PetState
{
    public override PetStateName StateName => PetStateName.Interact;

    private float moveSpeed = 1f;

    public override void OnEnter()
    {
        MoveArrived();
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
            transform.DOMove(hit, moveSpeed).OnComplete(() =>
            {
                CheckAroundInteract();
            });
        }
    }


    private void CheckAroundInteract()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider col in cols)
        {
            OutlineScript interact = col.GetComponent<OutlineScript>();
            if (interact == null) continue;

            interact.OnInteract();
            pet.State.ChangeState((int)PetStateName.Idle);
            return;
        }

    }
}
