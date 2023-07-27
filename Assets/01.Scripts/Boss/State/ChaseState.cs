using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossState
{
    public override BossStateName StateName => BossStateName.Chase;

    public override void OnEnter()
    {
        Debug.Log("Chase");
        if(boss.Target == null)
        {
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
