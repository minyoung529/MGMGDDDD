using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class ClockBoss : BossScript
{
    [Header("체력 관련")]
    [SerializeField] private float maxHp;
    private float curHp;
    private bool isInvincible;

    [Header("이벤트")]
    [SerializeField] private UnityEvent onEncounter;
    [SerializeField] private UnityEvent onPageChange;
    [SerializeField] private UnityEvent onDie;

    [Header("스킬 관련")]
    [SerializeField] private BossPage[] pageList;
    private int skillCount;

    [Header("폭죽")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private FireworkMove fire;

    #region abstract 구현부
    public override BossPage[] PageList => pageList;
    protected override Action OnEncounter => onEncounter.Invoke;

    private Animator anim;
    public override Animator Anim => anim;
    private int hash_tDamaged = Animator.StringToHash("tDamaged");

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Test")]
    public void Test() {
        Encounter();
        CallNextSkill();
    }

    #region 피격 관련
    public override void GetDamage(float damage) {
        if (isInvincible) return;
        Debug.Log(curHp);
        StartCoroutine(InvinsibleTimer(0.1f));
        CurPage.CurSkill.StopSkill();
        anim.SetTrigger(hash_tDamaged);
        curHp -= damage;
        if (curHp <= 0) {
            Die();
            return;
        }
        else if (curHp <= 0.5 * maxHp) {
            PageChange();
        }
        else
            CallNextSkill();
    }

    private IEnumerator InvinsibleTimer(float time) {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    [ContextMenu("PageChange")]
    protected override void PageChange() {
        onPageChange?.Invoke();
        CurPage.Reinforce(true);
    }

    protected override void Die() {
        onDie?.Invoke();
    }
    #endregion

    public override void CallNextSkill() {
        StartCoroutine(WaitForSkill());
    }
    #endregion

    private IEnumerator WaitForSkill() {
        yield return new WaitForSeconds(Random.Range(CurPage.TimeToNextSkill - CurPage.TTNRandomRange, CurPage.TimeToNextSkill + CurPage.TTNRandomRange));
        if (skillCount >= 2) {
            CurPage.SelectSkill(2);
            skillCount = 0;
        }
        else {
            CurPage.SelectSkill();
            skillCount++;
            if (!fire.gameObject.activeSelf && Random.Range(0, 2) > 0) {
                fire.gameObject.SetActive(true);
                fire.transform.position = spawnPoint.position;
            }
        }
        CurPage.Execute();
    }

    public override void ResetBoss() {
        CurPage.CurSkill.StopSkill();
        CurPage.Reinforce(false);
    }
}
