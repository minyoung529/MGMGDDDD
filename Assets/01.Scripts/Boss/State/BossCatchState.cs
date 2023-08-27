using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCatchState : BossState
{
    private Cupboard[] cupboards;


    public override BossStateName StateName => BossStateName.Catch;
    private float delayTime = 5f;

    private void Awake()
    {
        cupboards = FindObjectsOfType<Cupboard>();
    }
    public override void OnEnter()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(BossStateName.Patrol);
            return;
        }

        foreach (var cupboard in cupboards)
        {
            if (cupboard.PlayerIn)
            {
                boss.ChangeState(BossStateName.Patrol);
                return;
            }
        }

        Pet pet = boss.Target.GetComponent<Pet>();

        // ���� �� ���̶��
        if (pet != null)
        {
            // �̹� ��Ҵٸ� ���� �ʴ´�
            if (!boss.CatchingPet.IsContain(pet))
            {
                boss.ChangeState(BossStateName.PetCatch);
            }
            else
            {
                boss.ChangeState(BossStateName.Patrol);
            }
        }
        else
        {
            // Game Over
            if (boss.Target.transform == GameManager.Instance.PlayerController.transform)
            {

                EventManager.TriggerEvent(EventName.BossFail);
            }

            // TEMP
            boss.ChangeState(BossStateName.Patrol);
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {

    }
}
