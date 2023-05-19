using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;

public class OilPet : Pet
{
    [SerializeField] private OilSkillState skill;
    public OilSkillState SkillState { get { return skill; } }

    private void Start()
    {
        Event.StartListening((int)PetEventName.OnInteractEnd, InteractEvent);
    }

    public override void ResetPet()
    {
        base.ResetPet();
    }

    private void InteractEvent()
    {
        State.ChangeState((int)PetStateName.Idle);
        SetInteractNull();
    }

    private void OnDisable()
    {
        Event.StopListening((int)PetEventName.OnInteractEnd, InteractEvent);
    }
}
