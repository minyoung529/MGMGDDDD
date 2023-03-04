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

    #region abstract 구현부
    [SerializeField] private BossPage[] pageList;
    public override BossPage[] PageList => pageList;
    protected override Action OnEncounter => onEncounter;

    private Animator anim;
    public override Animator Anim => throw new NotImplementedException();

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Test")]
    public void Test() {
        Encounter();
        CallNextSkill();
    }

    public override void GetDamage() {
        curHp--;
        if (curHp <= 0) {
            Die();
            return;
        }
        //다음 페이지가 존재하고 해당 조건의 체력에 도달했을때
        if (pageIndex < PageList.Length - 1 && curHp / maxHp <= pageList[pageIndex + 1].ConditionHp) {
            PageChange();
        }
    }
    protected override void PageChange() {
        onPageChange?.Invoke();
        pageIndex++;
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
