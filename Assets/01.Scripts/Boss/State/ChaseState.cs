using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossState
{
    public override BossStateName StateName => BossStateName.Chase;
    private float maxDistance = 20.0f;

    public override void OnEnter()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(BossStateName.Patrol);
        }
    }

    public override void OnExit()
    {
        boss.Agent.ResetPath();
    }

    public override void OnUpdate()
    {
        boss.Agent.SetDestination(boss.Target.position);

        float distance = Vector3.Distance(boss.transform.position, boss.Target.position);
        if (distance > maxDistance)
        {
            boss.ChangeState(BossStateName.Patrol);
        }
        if (distance <= boss.Agent.stoppingDistance)
        {
            boss.ChangeState(BossStateName.Idle);
        }
    }
}
