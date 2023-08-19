using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunState : BossState
{
    public override BossStateName StateName => BossStateName.Stun;

    private const float stunTime = 10f;
    private float curTime = 0f;

    private bool stunning = false;

    public override void OnEnter()
    {
        curTime = stunTime;
        boss.Anim.ChangeAnimation(BossAnimType.Stun);
        stunning = true;
    }

    public override void OnExit()
    {
        stunning = false;
    }

    public override void OnUpdate()
    {
        if(stunning)
        {
            curTime -= Time.deltaTime;
            if (curTime < 0f) boss.ChangeState(BossStateName.Patrol);
        }
    }
}
