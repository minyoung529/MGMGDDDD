using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyState : PetState
{
    public override PetStateName StateName => PetStateName.Sticky;

    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private Transform stickyParent;

    private Transform originalParent = null;
    private Sticky stickyObject = null;
    private Vector3 stickyOffset;
    private Quaternion origianalRotation;

    private Sticky sticky;


    public override void OnEnter()
    {
        transform.DOKill();

        stickyObject = sticky;
        skillEffect.Play();

        if (sticky.CanMove)
        {
            pet.Event.StartListening((int)PetEventName.OnSetDestination, OnMove);
        }

        StartCoroutine(DelayParent());

        // SET ORIGINAL PARENT & PARENT

        // SET VARIABLE
        //originalParent = stickyObject.MovableRoot.parent;
        //stickyOffset = stickyObject.MovableRoot.position - stickyParent.position;
        //origianalRotation = stickyObject.MovableRoot.rotation;


        // Hey Minyoung~ It is StickyAxis ERROR!!
        // stickyObject.OnSticky(this);
    }

    public override void OnExit()
    {
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;

        if (stickyObject)
        {
            stickyObject.MovableRoot.SetParent(originalParent);
            stickyObject.NotSticky();
        }
    }

    public override void OnUpdate()
    {

    }

    private IEnumerator DelayParent()
    {
        yield return null;

        originalParent = stickyObject.MovableRoot.parent;
        stickyOffset = stickyObject.MovableRoot.position - stickyParent.position;
        origianalRotation = stickyObject.MovableRoot.rotation;
        stickyObject.MovableRoot.SetParent(stickyParent);
        stickyObject.MovableRoot.DOLocalMove(new Vector3(0f, 1f, 0f), 1f);
    }

    private void OnMove()
    {
        pet.State.ChangeState((int)PetStateName.Move);
    }
}
