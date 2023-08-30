using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Net.NetworkInformation;

public class StickySkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    [SerializeField] private Transform explosion;
    [SerializeField] private UnityEvent onBillow;
    [SerializeField] private UnityEvent onExitBillow;

    [SerializeField]
    private SkillVisual enterVisual;
    [SerializeField]
    private SkillVisual exitVisual;

    private Vector3 smallDirection;

    public override void OnEnter()
    {
        Debug.Log("Skill");

        if (SelectedObject.CurInteractObject &&
       (SelectedObject.CurInteractObject.PetType & PetFlag.StickyPet) != 0)
        {
            pet.State.ChangeState(pet.State.BeforeStateIndex);
            return;
        }

        pet.Skilling = true;

        pet.Event.StartListening((int)PetEventName.OnSkillCancel, OffBillow);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StartListening((int)PetEventName.OnOffPing, PingUp);

        OnBillow();
    }

    public void OnExplosionStart()
    {
        pet.Event.StopListening((int)PetEventName.OnSkillCancel, OffBillow);
    }

    private void PingUp()
    {
        pet.StopPing();
    }

    public override void OnExit()
    {
        if (SelectedObject.CurInteractObject) return;

        pet.Skilling = false;
        exitVisual.Trigger();

        explosion.gameObject.SetActive(false);
        onExitBillow?.Invoke();

        pet.Event.StopListening((int)PetEventName.OnOffPing, PingUp);
        pet.Event.StopListening((int)PetEventName.OnSkillCancel, OffBillow);
        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }


    private void Awake()
    {
        explosion.gameObject.SetActive(false);
        smallDirection = transform.forward;

        enterVisual.ListenCompleteEvent(() => pet.Event.TriggerEvent((int)PetEventName.OnSkillComplete));
        enterVisual.ListenCompleteEvent(() => explosion.gameObject.SetActive(true));
        exitVisual.ListenCompleteEvent(() => pet.Event.TriggerEvent((int)PetEventName.OnSkillOffComplete));
    }

    #region Listen

    private void OnRecall()
    {
        pet.State.ChangeState((int)PetStateName.Recall);
    }

    #endregion

    #region Skill
    private void OnBillow()
    {
        BillowAction();
        enterVisual.Trigger();
        onBillow?.Invoke();
    }

    private void OffBillow()
    {
        pet.State.ChangeState((int)PetStateName.Idle);
    }

    private void BillowAction()
    {
        if (smallDirection.sqrMagnitude != 0f)
        {
            transform.forward = smallDirection;
        }
        smallDirection = Vector3.zero;
    }

    private void SetBillow(Vector3 dir)
    {
        smallDirection = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        Sticky stickyObject = other.GetComponent<Sticky>();
        if (stickyObject != null)
        {
            SetBillow(other.transform.forward);
        }
    }

    public override void OnUpdate()
    {
    }
    #endregion

}
