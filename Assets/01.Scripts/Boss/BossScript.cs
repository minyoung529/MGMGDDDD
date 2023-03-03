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
