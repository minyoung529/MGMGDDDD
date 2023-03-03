using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBoss : BossScript
{
    [SerializeField] private float maxHp;
    private float curHp;

    [SerializeField] private Action onEncounter;
    [SerializeField] private Action onPageChange;
    [SerializeField] private Action onDie;

    #region abstract ±¸ÇöºÎ
    [SerializeField] private BossPage[] skillList;
    public override BossPage[] SkillList => skillList;

    public override void Encounter() {
        onEncounter?.Invoke();
    }

    public override void GetDamage() {
        curHp--;
        if (curHp <= 0) {
            Die();
            return;
        }
        if (pageIndex < SkillList.Length)
    }
    protected override void PageChange() {
        onPageChange?.Invoke();
        pageIndex++;
    }

    protected override void Die() {
        onDie?.Invoke();
    }

    protected override void CallNextSkill() {
        throw new NotImplementedException();
    }
    #endregion
}
