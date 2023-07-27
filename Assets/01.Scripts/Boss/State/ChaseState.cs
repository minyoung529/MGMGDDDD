using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossState
{
    public override BossStateName StateName => BossStateName.Chase;
    private float maxDistance = 15.0f;

    public override void OnEnter()
    {
        if(boss.Target == null)
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
        if(distance >= maxDistance) {
            boss.ChangeState(BossStateName.Idle);
        }
        if(distance <= boss.Agent.stoppingDistance)
        {
            Debug.Log("ÀâÈû");
            boss.ChangeState(BossStateName.Idle);
        }
    }
}
