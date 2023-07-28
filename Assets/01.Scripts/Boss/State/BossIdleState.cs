using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    public override BossStateName StateName => BossStateName.Idle;

    private float idleTime = 3.0f;

    public override void OnEnter()
    {
        StartCoroutine(IdleTime());
    }

    private IEnumerator IdleTime()
    {
        boss.Anim.ChangeAnimation(BossAnimType.Idle);
        yield return new WaitForSeconds(idleTime);
        boss.ChangeState(BossStateName.Patrol);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}
