using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BossScript))]
public abstract class BossSkill : MonoBehaviour
{
    protected BossScript bossScript;
    public abstract float ChanceFactor { get; }

    protected virtual void Awake() {
        bossScript = GetComponent<BossScript>();
    }

    public abstract void ExecuteSkill();

    //선 딜레이 시작
    public abstract void PreDelay();

    //선 딜레이 종료
    public abstract void HitTime();

    //후 딜레이 시작
    public abstract void PostDelay();

    //후 딜레이 종료
    public virtual void End() {
        bossScript.UseSkill();
    }

    /// <summary>
    /// 스킬 강제 종료
    /// </summary>
    public abstract void StopAttack();
}
