using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCatchState : BossState
{
    public override BossStateName StateName => BossStateName.PetCatch;
    [SerializeField] private CatchingPet catchingPet;

    public override void OnEnter()
    {
        Pet pet = boss.Target.GetComponent<Pet>();
        pet.State.ChangeState((int)PetStateName.Idle);  // �� ���� ����
        pet.SetNavIsStopped(true);
        pet.SetNavEnabled(false);
        pet.Rigid.velocity = Vector3.zero;

        // �� ���ֱ�
        PetManager.Instance.DeletePet(pet.GetPetType);

        boss.Agent.isStopped = true;

        catchingPet.EquipPet(pet, OnEndAnimation);
        boss.ResetTarget();
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
