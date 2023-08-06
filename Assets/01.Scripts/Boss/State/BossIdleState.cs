using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    public override BossStateName StateName => BossStateName.Idle;

    private float idleTime = 3.0f;

    public override void OnEnter()
    {
        Debug.Log("Idle");
        boss.Agent.isStopped = true;
        StartCoroutine(IdleTime());
    }

    private IEnumerator IdleTime()
    {
        yield return new WaitForSeconds(idleTime);
        boss.ChangeState(BossStateName.Patrol);
    }

    public override void OnExit()
    {
        boss.Agent.isStopped = false;
    }

    public override void OnUpdate()
    {
    }
}
