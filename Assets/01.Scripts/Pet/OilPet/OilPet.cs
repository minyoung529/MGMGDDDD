using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using Cinemachine;
using UnityEngine.AI;
using System.Reflection;
using UnityEngine.UIElements;
using System;

public class OilPet : Pet
{
    [SerializeField] GameObject oilSkill;
    [SerializeField] GameObject fireBurnParticle;
    [SerializeField] Transform parentController;
    [SerializeField] Transform splatGunNozzle;

    ParticleSystem inkParticle;

    private bool isParticleOn;

    [SerializeField]
    private NavMeshAgent pathAgent;
    private OilPetSkill oilPetSkill = new OilPetSkill();
    private bool isSkilling;
    private bool pauseSkilling = false;
    protected bool isSkillDragging;

    #region Property
    public Action OnStartSkill { get; set; }
    public Action OnEndSkill { get; set; }

    public bool IsDirectSpread { get; set; } = true;

    public OilPetSkill OilPetSkill => oilPetSkill;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //inkParticle = parentController.transform.GetComponentInChildren<ParticleSystem>();

        oilPetSkill?.Init(GetComponentInChildren<PaintingObject>(), GetComponent<LineRenderer>(), pathAgent, agent);
    }
    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();
    }
    #endregion

    #region Skill

    // Active skill
    public override void Skill()
    {
        if (IsCoolTime || isSkillDragging || isSkillDragging || pauseSkilling) return;
        base.Skill();

        OnStartSkill?.Invoke();
        isSkillDragging = true;
        isSkilling = true;
        oilPetSkill.OnClickSkill();
    }
    private void ParticlePlay(Vector3 hit)
    {
        if (isParticleOn) return;
        VisualPolish(hit);
        StartCoroutine(InkParticle());
    }
    IEnumerator InkParticle()
    {
        isParticleOn = true;
        inkParticle.Play();
        yield return new WaitForSeconds(2f);
        inkParticle.Stop();
        isParticleOn = false;
    }
    void VisualPolish(Vector3 point)
    {
        if (!DOTween.IsTweening(parentController))
        {
            parentController.DOComplete();
            Vector3 forward = -parentController.forward;
            Vector3 localPos = parentController.localPosition;
            parentController.DOLocalMove(localPos - new Vector3(0, .2f, 0), .03f)
                .OnComplete(() => parentController.DOLocalMove(localPos, .1f).SetEase(Ease.OutSine));
        }

        if (!DOTween.IsTweening(splatGunNozzle))
        {
            splatGunNozzle.DOComplete();
            splatGunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);
        }
    }
    #endregion

    public void SpreadOil()
    {
        if (isSkilling && (!isMouseMove || IsDirectSpread))
        {
            oilPetSkill.StartSpreadOil(() => SetNavIsStopped(true), () => { SetTarget(null); SetNavIsStopped(false); ResetSkill(); });
        }
    }

    public override void SkillUp()
    {
        base.SkillUp();

        if (!isSkilling || !isSkillDragging) return;

        if (IsDirectSpread)
        {
            SetDestination(oilPetSkill.StartPoint);
            onArrive += SpreadOil;
        }
        OnEndSkill?.Invoke();

        agent.isStopped = false;
        isSkillDragging = false;
        pauseSkilling = false;
    }

    protected void ResetSkill()
    {
        isSkilling = false;
        SetDestination(transform.position);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!pauseSkilling)
        {
            oilPetSkill.Update(isSkilling, isSkillDragging);
        }
    }

    public void PauseSkill(bool pause)
    {
        pauseSkilling = pause;
    }
}
