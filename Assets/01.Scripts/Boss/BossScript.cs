using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BossScript : MonoBehaviour
{
    public abstract Animator Anim { get; }
    protected abstract Action OnEncounter { get; }

    #region 스킬 관련 변수
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
    /// 스킬이 종료되면 호출되는 함수
    /// </summary>
    public abstract void CallNextSkill();

    #region 애니메이션 이벤트 함수
    //선 딜레이 시작
    public virtual void PreDelay() {
        CurPage.CurSkill.PreDelay();
    }
    //선 딜레이 종료
    public virtual void HitTime() {
        CurPage.CurSkill.HitTime();
    }
    //후 딜레이 시작
    public virtual void PostDelay() {
        CurPage.CurSkill.PostDelay();
    }
    //후 딜레이 종료
    public virtual void SkillEnd() {
        CurPage.CurSkill.SkillEnd();
        CallNextSkill();
    }
    #endregion
}
