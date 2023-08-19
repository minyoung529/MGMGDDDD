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

        // 잡은 게 펫이라면
        if (pet != null)
        {
            boss.ChangeState(BossStateName.PetCatch);
        }
        else
        {
            Debug.Log("Catch");
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
        boss.ChangeState(BossStateName.Idle);
    }
}
