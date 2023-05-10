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

    StickyPet stickyPet;
    private Sticky sticky;

    private void Awake()
    {
        stickyPet = transform.parent.GetComponent<StickyPet>();
    }

    public override void OnEnter()
    {
        sticky = stickyPet.StickyObject;
        if(sticky == null) GetStickyAround();
        
        transform.DOKill();
        skillEffect.Play();

        if (sticky.CanMove)
            pet.Event.StartListening((int)PetEventName.OnSetDestination, OnMove);

        StartCoroutine(DelayParent());
        sticky.OnSticky(stickyPet);
    }

    private void GetStickyAround()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in cols)
        {
            Sticky s = col.GetComponent<Sticky>();
            if (s)
            {
                sticky = s;
                break;
            }
        }
    }

    public override void OnExit()
    {
       if(sticky.CanMove)
        {
            stickyPet.StickyObject = null;
            pet.Event.StopListening((int)PetEventName.OnSetDestination, OnMove);
        }
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
