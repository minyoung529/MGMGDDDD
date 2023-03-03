using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BossScript : MonoBehaviour
{
    public abstract BossPage[] SkillList { get; }
    protected int pageIndex = 0;
    public BossPage CurPage => SkillList[pageIndex];

    public abstract void Encounter();
    public abstract void GetDamage();
    protected abstract void PageChange();
    protected abstract void Die();
    protected abstract void CallNextSkill();

    #region �ִϸ��̼� �̺�Ʈ �Լ�
    //�� ������ ����
    public virtual void PreDelay() {
        CurPage.CurSkill.PreDelay();
    }
    //�� ������ ����
    public virtual void HitTime() {
        CurPage.CurSkill.HitTime();
    }
    //�� ������ ����
    public virtual void PostDelay() {
        CurPage.CurSkill.PostDelay();
    }
    //�� ������ ����
    public virtual void SkillEnd() {
        CurPage.CurSkill.SkillEnd();
        CallNextSkill();
    }
    #endregion
}
