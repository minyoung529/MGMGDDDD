using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBoss : BossScript
{
    public override float MaxHp => throw new NotImplementedException();
    public override float CurHp { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
    public override float TimeToNextSkill => 3f;

    [SerializeField] private BossSkill[] skills;
    public override BossSkill[] Skills => skills;

    protected override Action OnEncounter => throw new NotImplementedException();

    protected override Action OnDie => throw new NotImplementedException();
}
