using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StickyState : PetState
{
    public override PetStateName StateName => PetStateName.Sticky;

    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private Transform stickyParent;

    private Transform originalParent = null;
    private Quaternion origianalRotation;

    private StickyPet stickyPet;
    private Sticky sticky;

    private void Awake()
    {
        stickyPet = pet.State.Parent.GetComponent<StickyPet>();
    }

    public override void OnEnter()
    {
            pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);

        GetStickyAround();
        if (sticky == null)
        {
            pet.State.ChangeState((int)PetStateName.Idle);
            return;
        }
        
        if (sticky.CanMove)
        {
            pet.State.BlockState((int)PetStateName.Skill);
            pet.Event.StartListening((int)PetEventName.OnSetDestination, OnMove);
        }

        OnSticky();
    }

    private void OnSticky()
    {
        skillEffect.Play();
        sticky.StartListeningNotSticky(OffSticky);
        sticky.OnSticky(stickyPet);
        StartCoroutine(DelayParent());
    }

    private void GetStickyAround()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider col in cols)
        {
            sticky = col.GetComponent<Sticky>();
            if (sticky)  break;
        }
    }

    public override void OnExit()
    {
        if(sticky.CanMove)
        {
            pet.Event.StopListening((int)PetEventName.OnSetDestination, OnMove);
        }
    }

    public override void OnUpdate()
    {

    }

    private void OffSticky()
    {
        if (sticky == null) return;
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;

        sticky.MovableRoot.SetParent(originalParent);
        pet.State.BlockState((int)PetStateName.Skill);
        sticky = null;
    }

    private IEnumerator DelayParent()
    {
        yield return null;

        originalParent = sticky.MovableRoot.parent;
        origianalRotation = sticky.MovableRoot.rotation;
        sticky.MovableRoot.SetParent(stickyParent);
        sticky.MovableRoot.DOLocalMove(new Vector3(0f, 1f, 0f), 1f);
    }

    private void OnMove()
    {
        pet.State.ChangeState((int)PetStateName.Move);
    }
    private void OnRecall()
    {
        pet.State.ChangeState((int)PetStateName.Recall);
    }
}
