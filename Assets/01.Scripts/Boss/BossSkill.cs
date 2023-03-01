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

    //�� ������ ����
    public abstract void PreDelay();

    //�� ������ ����
    public abstract void HitTime();

    //�� ������ ����
    public abstract void PostDelay();

    //�� ������ ����
    public virtual void End() {
        bossScript.UseSkill();
    }

    /// <summary>
    /// ��ų ���� ����
    /// </summary>
    public abstract void StopAttack();
}
