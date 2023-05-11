using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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

        OnSkill();
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnSkillKeyUp, SkillUp);
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
        if (isSkillDragging || pauseSkilling) return;

        OnStartSkill?.Invoke();
        isSkillDragging = true;
        pet.Skilling = true;
        oilPetSkill.OnClickSkill();
    }
    #endregion

    public void SpreadOil()
    {
        if (pet.Skilling && IsDirectSpread)
        {
            oilPetSkill.StartSpreadOil(() => pet.SetNavIsStopped(true), OnEndPath);
            onOilSkill?.Invoke();
        }
    }

    private void OnEndPath()
    {
        pet.SetTarget(null);
        pet.SetNavIsStopped(false);
        ResetSkill();
        pet.IsMovePointLock = false;
    }

    private void SkillUp()
    {
        if (!pet.Skilling || !isSkillDragging) return;

        pet.Agent.isStopped = false;
        isSkillDragging = false;
        pet.IsMovePointLock = true;
        pauseSkilling = false;

        if (IsDirectSpread)
        {
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

    private void KillSkill()
    {
        oilPetSkill.KillSkill();
        OnEndPath();
    }

    private void ResetSkill()
    {
        pet.Skilling = false;
        pet.SetTargetPlayer();

        pet.Event.StopListening((int)PetEventName.OnArrive, SpreadOil);
        pet.Event.StopListening((int)PetEventName.OnStop, KillSkill);
    }
}
