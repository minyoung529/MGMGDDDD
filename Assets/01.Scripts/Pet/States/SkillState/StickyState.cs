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
    private Vector3 stickyOffset;
    private Quaternion origianalRotation;

    private Sticky sticky;

    public override void OnEnter()
    {
        sticky = transform.parent.GetComponent<StickyPet>().StickyObject;
        transform.DOKill();
        skillEffect.Play();

        if(sticky == null) Debug.Log("NULL");

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

        StickyPet stickyPet = transform.parent.GetComponent<StickyPet>();
        if (stickyPet) sticky.OnSticky(stickyPet);
    }

    public override void OnExit()
    {
       
            pet.Event.StopListening((int)PetEventName.OnSetDestination, OnMove);
    }

    public override void OnUpdate()
    {

    }

    private void OffSticky()
    {
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;

        if (sticky)
        {
            sticky.MovableRoot.SetParent(originalParent);
            sticky.NotSticky();
        }
    }

    private IEnumerator DelayParent()
    {
        yield return null;
        originalParent = sticky.MovableRoot.parent;
        stickyOffset = sticky.MovableRoot.position - stickyParent.position;
        origianalRotation = sticky.MovableRoot.rotation;
        sticky.MovableRoot.SetParent(stickyParent);
        sticky.MovableRoot.DOLocalMove(new Vector3(0f, 1f, 0f), 1f);
    }

    private void OnMove()
    {
        pet.State.ChangeState((int)PetStateName.Move);
    }
}
