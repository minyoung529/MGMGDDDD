using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class ClockBoss : BossScript
{
    [SerializeField] private float maxHp;
    private float curHp;

    [SerializeField] private Action onEncounter;
    [SerializeField] private Action onPageChange;
    [SerializeField] private Action onDie;

    #region abstract ±¸ÇöºÎ
    [SerializeField] private BossPage[] pageList;
    public override BossPage[] PageList => pageList;
    protected override Action OnEncounter => onEncounter;

    private Animator anim;
    public override Animator Anim => anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Test")]
    public void Test() {
        Encounter();
        CallNextSkill();
    }

    public override void GetDamage(float damage) {
        curHp -= damage;
        if (curHp <= 0) {
            Die();
            return;
        }
        if (curHp <= 0.5 * maxHp) {
            PageChange();
        }
    }

    [ContextMenu("Test2")]
    protected override void PageChange() {
        onPageChange?.Invoke();
        foreach(BossPage item in pageList) {
            item.Reinforce();
        }
    }

    protected override void Die() {
        onDie?.Invoke();
    }

    public override void CallNextSkill() {
        StartCoroutine(WaitForSkill());
    }
    #endregion

    private IEnumerator WaitForSkill() {
        yield return new WaitForSeconds(Random.Range(CurPage.TimeToNextSkill - CurPage.TTNRandomRange, CurPage.TimeToNextSkill + CurPage.TTNRandomRange));
        CurPage.SelectSkill();
        CurPage.Execute();
    }
}
