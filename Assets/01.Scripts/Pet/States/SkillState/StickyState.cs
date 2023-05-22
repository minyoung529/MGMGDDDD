using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyState : PetState
{
    public override PetStateName StateName => PetStateName.Sticky;

    [SerializeField] private ParticleSystem skillEffect;
    [SerializeField] private Transform stickyParent;
    [SerializeField] private UnityEvent stickyEvent;

    private Transform originalParent = null;
    private Quaternion origianalRotation;

    private StickyPet stickyPet;
    private Sticky sticky;

    private void Start()
    {
        stickyPet = pet.State.Parent.GetComponent<StickyPet>();
    }

    public override void OnEnter()
    {
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);

        sticky = SelectedObject.CurInteractObject.GetComponent<Sticky>();
        if (sticky == null)
        {
            pet.State.ChangeState((int)PetStateName.Idle);
            return;
        }
        
        if (sticky.CanMove)
        {
            pet.Event.StartListening((int)PetEventName.OnSetDestination, OnMove);
        }

        OnSticky();
    }

    private void OnSticky()
    {
        skillEffect.Play();
        stickyEvent?.Invoke();
        pet.State.BlockState((int)PetStateName.Skill);

        sticky.StartListeningNotSticky(OffSticky);
        sticky.OnSticky(stickyPet);
        StartCoroutine(DelayParent());
    }

    public override void OnExit()
    {
        if(sticky.CanMove)
        {
            pet.Event.StopListening((int)PetEventName.OnSetDestination, OnMove);
        }
            pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnUpdate()
    {

    }

    private void OffSticky()
    {
        if (sticky == null) return;

        pet.SetInteractNull();
        pet.Rigid.isKinematic = false;
        pet.Rigid.useGravity = true;

        sticky.MovableRoot.SetParent(originalParent);
        pet.State.UnBlockState((int)PetStateName.Skill);

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
