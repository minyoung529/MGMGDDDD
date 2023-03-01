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
    /// �������� ������ ����� ��ų ����
    /// </summary>
    public void SelectSkill() {
        float rand = Random.Range(0, totalChance);
        int index = 0;
        for (float sum = Skills[0].ChanceFactor; rand > sum; sum += Skills[++index].ChanceFactor) ;
        //���� 3���� ��ų Ȯ���� 1 : 1 : 1 �̶�� �� 3�� ���� �� 0 ~ 3 ������ ������ ���� ���ϰ� 1���϶�� ù ��° ��ų�� 2���϶�� �� ��° ��ų�� ����Ѵ�.
        skillIndex = index;
    }

    /// <summary>
    /// �ε����� �ش��ϴ� ��ų ����
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
