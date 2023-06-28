using System.Diagnostics;
using System.Runtime.CompilerServices;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

using Debug = UnityEngine.Debug;

public class OilSkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    protected bool isSkillDragging;
    private bool pauseSkilling = false;

    [SerializeField] Transform parentController;
    [SerializeField] private NavMeshAgent pathAgent;
    private OilPetSkill oilPetSkill = new OilPetSkill();

    [SerializeField]
    private UnityEvent onOilSkill;

    private Transform prevTransform;

    #region Property
    public Action OnStartSkill { get; set; }
    public Action OnEndSkill { get; set; }

    public bool IsDirectSpread { get; set; } = true;

    public OilPetSkill SkillData => oilPetSkill;
    public Vector3 SkillStartPoint => SkillData.StartPoint;
    #endregion

    private void Start()
    {
        oilPetSkill?.Init(GetComponentInChildren<PaintingObject>(), GetComponentInChildren<LineRenderer>(), pathAgent, pet.Agent);
    }

    public override void OnEnter()
    {
        pet.Event.StartListening((int)PetEventName.OnSkillKeyUp, SkillUp);
        pet.Event.StartListening((int)PetEventName.OnSkillCancel, KillSkill);

        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, KillSkill);
        pet.Event.StartListening((int)PetEventName.OnHold, KillSkill);
        pet.Event.StartListening((int)PetEventName.OnOffPing, PingUp);

        OnSkill();
    }

    public override void OnExit()
    {
    }



    public override void OnUpdate()
    {
        if (!pauseSkilling)
        {
            oilPetSkill.Update(pet.Skilling, isSkillDragging);
        }
    }

    #region Skill
    // Active skill
    private void OnSkill()
    {
        if (isSkillDragging) return;

        prevTransform = pet.Target;
        OnStartSkill?.Invoke();
        isSkillDragging = true;
        pet.Skilling = true;
        oilPetSkill.OnClickSkill();
    }
    #endregion

    private void PingUp()
    {
        pet.StopPing();
    }

    public void SpreadOil()
    {
        if (pet.Skilling || pauseSkilling)
        {
            oilPetSkill.StartSpreadOil(() => pet.SetNavIsStopped(true), OnEndPath);
            pet.Event.TriggerEvent((int)PetEventName.OnDrawStart);
            onOilSkill?.Invoke();
            StopListeningEvents();
            pet.Event.StopListening((int)PetEventName.OnSkillKeyUp, SkillUp);
        }
    }

    private void OnEndPath()
    {
        ResetSkill();

        pet.SetNavIsStopped(false);
        pet.IsMovePointLock = false;
    }

    private void SkillUp()
    {
        if (!isSkillDragging) return;

        pet.Agent.isStopped = false;
        isSkillDragging = false;
        pet.IsMovePointLock = true;
        pauseSkilling = false;

        if (IsDirectSpread)
        {
            pet.SetTarget(null);
            pet.SetDestination(oilPetSkill.StartPoint, stopDistance: 0);

            pet.State.ChangeState((int)PetStateName.Move);
            pet.Event.StartListening((int)PetEventName.OnArrive, SpreadOil);
            pet.Event.StartListening((int)PetEventName.OnStop, KillSkill);
        }
        OnEndSkill?.Invoke();
    }

    public void PauseSkill(bool pause)
    {
        pauseSkilling = pause;
    }

    public void KillSkill()
    {
        isSkillDragging = false;
        pet.Skilling = false;

        pauseSkilling = false;

        oilPetSkill.KillSkill();
        OnEndPath();

        pet.Event.StopListening((int)PetEventName.OnRecallKeyPress, KillSkill);
        pet.Event.StopListening((int)PetEventName.OnHold, KillSkill);
        pet.Event.StopListening((int)PetEventName.OnSkillCancel, KillSkill);

        pet.State.ChangeState((int)PetStateName.Idle);
        pet.Rigid.DOKill();
        transform.DOKill();
    }

    private void ResetSkill()
    {
        pet.Skilling = false;

        if (prevTransform == GameManager.Instance.PlayerController.transform)
        {
            pet.SetTargetPlayer();
        }
        else
        {
            pet.SetTarget(prevTransform);
        }

        StopListeningEvents();
        pet.Event.TriggerEvent((int)PetEventName.OnSkillComplete);
    }

    private void StopListeningEvents()
    {
        pet.Event.StopListening((int)PetEventName.OnArrive, SpreadOil);
        pet.Event.StopListening((int)PetEventName.OnStop, KillSkill);
        pet.Event.StopListening((int)PetEventName.OnOffPing, PingUp);
    }
}