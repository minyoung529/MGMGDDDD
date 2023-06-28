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

    private void Start()
    {
        fire = pet.GetComponentInChildren<Fire>();
    }

    public override void OnEnter()
    {
    pet.Event.StartListening((int) PetEventName.OnOffPing, PingUp);
        StartCoroutine(FireBurn());
        onFire?.Invoke();
    }


    public override void OnExit()
    {
    pet.Event.StopListening((int) PetEventName.OnOffPing, PingUp);

    }

    public override void OnUpdate()
    {

    }

    public void OnSkill()
    {
        fire.Burn();
    }

    private void PingUp()
    {
        pet.StopPing();
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