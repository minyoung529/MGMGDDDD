using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCatchState : BossState
{
    public override BossStateName StateName => BossStateName.Catch;
    private float delayTime = 5f;

    public override void OnEnter()
    {
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
            boss.ResetTarget();
            StartCoroutine(CatchDelay());
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        
    }

    private IEnumerator CatchDelay()
    {
        yield return new WaitForSeconds(delayTime);
        boss.ChangeState(BossStateName.Patrol);
    }
}
