using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireSkillState : PetState
{
    public override PetStateName StateName => PetStateName.Skill;

    private Fire fire;

    [SerializeField]
    private UnityEvent onFire;

    private void Awake()
    {
        fire = GetComponentInParent<Fire>();
    }

    public override void OnEnter()
    {
        StartCoroutine(FireBurn());
        onFire?.Invoke();
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
        pet.Event.TriggerEvent((int)PetEventName.OnSkillComplete);
        pet.State.ChangeState((int)PetStateName.Idle);
    }

    private IEnumerator FireBurn()
    {
        OnSkill();
        yield return new WaitForSeconds(pet.petInform.skillDelayTime);
        OffSkill();
    }

}