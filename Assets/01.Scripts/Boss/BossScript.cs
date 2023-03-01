using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public abstract class BossScript : MonoBehaviour
{
    public abstract float MaxHp { get; }
    public abstract float CurHp { get; protected set; }
    public abstract float TimeToNextSkill { get; }

    public abstract BossSkill[] Skills { get; }
    protected int skillIndex = 0;
    protected float totalChance = 0;

    protected abstract Action OnEncounter { get; }
    protected abstract Action OnDie { get; }

    protected virtual void Awake() {
        for(int i = 0; i < Skills.Length; i++) {
            totalChance += Skills[i].ChanceFactor;
        }
    }

    public virtual void Encounter() {
        OnEncounter.Invoke();
    }

    /// <summary>
    /// 랜덤으로 보스가 사용할 스킬 선택
    /// </summary>
    public void SelectSkill() {
        float rand = Random.Range(0, totalChance);
        int index = 0;
        for (float sum = Skills[0].ChanceFactor; rand > sum; sum += Skills[++index].ChanceFactor) ;
        //만약 3개의 스킬 확률이 1 : 1 : 1 이라면 합 3을 구한 뒤 0 ~ 3 까지의 랜덤한 수를 구하고 1이하라면 첫 번째 스킬을 2이하라면 두 번째 스킬을 사용한다.
        skillIndex = index;
    }

    /// <summary>
    /// 인덱스에 해당하는 스킬 선택
    /// </summary>
    public void SelectSkill(int index) {
        skillIndex = index;
    }

    public virtual void UseSkill() {
        SelectSkill();
        StartCoroutine(Skill());
    }

    protected virtual IEnumerator Skill() {
        yield return new WaitForSeconds(Random.Range(TimeToNextSkill - 1, TimeToNextSkill + 1));
        Skills[skillIndex].ExecuteSkill();
    }

    public virtual void GetDamage() {
        CurHp--;
        if (CurHp <= 0)
            OnDie.Invoke();
    }
}
