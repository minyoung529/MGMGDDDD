using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BossScript : MonoBehaviour
{
    public abstract Animator Anim { get; }
    protected abstract Action OnEncounter { get; }

    #region ��ų ���� ����
    public abstract BossPage[] PageList { get; }
    protected int pageIndex = 0;
    public BossPage CurPage => PageList[pageIndex];
    #endregion

    public virtual void Encounter() {
        OnEncounter?.Invoke();
        for(int i = 0; i < PageList.Length; i++) {
            PageList[i].SetParent(this);
        }
    }
    public abstract void GetDamage();
    protected abstract void PageChange();
    protected abstract void Die();
    
    /// <summary>
    /// ��ų�� ����Ǹ� ȣ��Ǵ� �Լ�
    /// </summary>
    public abstract void CallNextSkill();

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
