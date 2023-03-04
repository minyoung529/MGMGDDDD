using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FallObj : BossSkill 
{
    [SerializeField] private Vector3 spawnRange;
    public override float ChanceFactor => 0;

    public override void ExecuteSkill() {
        throw new System.NotImplementedException();
    }

    public override void HitTime() {
        throw new System.NotImplementedException();
    }

    public override void PostDelay() {
        throw new System.NotImplementedException();
    }

    public override void PreDelay() {
        throw new System.NotImplementedException();
    }

    public override void SkillEnd() {
        throw new System.NotImplementedException();
    }

    public override void StopSkill() {
        throw new System.NotImplementedException();
    }
}
