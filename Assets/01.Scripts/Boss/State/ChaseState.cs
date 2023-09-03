using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossState
{
    public override BossStateName StateName => BossStateName.Chase;
    private float maxDistance = 20.0f;

    public override void OnEnter()
    {
        //boss.Anim.ChangeAnimation(BossAnimType.Run);
        boss.Anim.GetAnimator().SetBool("Chase", true);
    }

    public override void OnExit()
    {
        boss.Agent.ResetPath();
        boss.Anim.GetAnimator().SetBool("Chase", false);
    }

    public override void OnUpdate()
    {
        if (boss.Target == null)
        {
            boss.ChangeState(BossStateName.Patrol);
            return;
        }

        boss.Agent.SetDestination(boss.Target.position);

        //float distance = Vector3.Distance(boss.transform.position, boss.Target.position);
        //if (distance > maxDistance)
        //{
        //    boss.ChangeState(BossStateName.Patrol);
        //}
    }
}
