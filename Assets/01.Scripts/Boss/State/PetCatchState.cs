using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCatchState : BossState
{
    public override BossStateName StateName => BossStateName.PetCatch;

    public override void OnEnter()
    {
        Pet pet = boss.Target.GetComponent<Pet>();

        boss.Agent.isStopped = true;

        boss.CatchingPet.EquipPet(pet, OnEndAnimation);
        boss.ResetTarget();
        boss.Anim.GetAnimator().SetTrigger("PetCatch");
    }

    private void OnEndAnimation()
    {
        boss.ChangeState(BossStateName.Patrol);
    }

    public override void OnExit()
    {
        boss.Agent.isStopped = false;
        boss.ResetTarget();
    }

    public override void OnUpdate()
    {
    }
}
