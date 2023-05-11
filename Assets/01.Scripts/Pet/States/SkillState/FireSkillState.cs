using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    private Fire fire;

    [SerializeField]
    private SkillVisual skillVisual;

    private void Awake()
    {
        fire = GetComponentInParent<Fire>();
    }

    public override void OnEnter()
    {
        StartCoroutine(FireBurn());
        skillVisual.Trigger();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }

    public void OnSkill()
    {
        fire.Burn();
    }

    public void OffSkill()
    {
        fire.StopBurn();
        pet.State.ChangeState((int)PetStateName.Idle);
    }

    private IEnumerator FireBurn()
    {
        OnSkill();
        yield return new WaitForSeconds(pet.petInform.skillDelayTime);
        OffSkill();
    }

}