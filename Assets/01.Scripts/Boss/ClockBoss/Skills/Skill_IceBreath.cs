using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_IceBreath : BossSkill
{
    [SerializeField] private GameObject effect_1;
    [SerializeField] private GameObject effect_2;
    [SerializeField] private GameObject coll;

    [SerializeField] private float chanceFactor;
    public override float ChanceFactor => chanceFactor;

    private int count = 0;

    private int hash_tIceBreath = Animator.StringToHash("tIceBreath");
    private int hash_iRandom = Animator.StringToHash("iRandom");
    private int hash_iBreathSpeed = Animator.StringToHash("iBreathSpeed");

    public override void ExecuteSkill() {
        parent.Anim.SetInteger(hash_iRandom, Random.Range(0, 2));
        count = 1;
        if (isReinforce) {
            parent.Anim.SetFloat(hash_iBreathSpeed, 2);
            count = 3;
        }
        parent.Anim.SetTrigger(hash_tIceBreath);
    }

    public override void PreDelay() {
        count--;
        Debug.Log("Pre");
        effect_1.SetActive(true);
    }

    public override void HitTime() {
        effect_2.SetActive(true);
        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider() {
        yield return new WaitForSeconds(0.1f);
        coll.gameObject.SetActive(true);
    }

    public override void PostDelay() {
        effect_2.SetActive(false);
        coll.gameObject.SetActive(false);
    }

    public override void SkillEnd() {
        if (count > 0) {
            Debug.Log("end");
            parent.Anim.SetInteger(hash_iRandom, Random.Range(0, 2));
            parent.Anim.SetTrigger(hash_tIceBreath);
        }
        else {
            Debug.Log("³¡");
            effect_1.SetActive(false);
            parent.CallNextSkill();
        }
    }

    public override void StopSkill() {
        effect_1.gameObject.SetActive(false);
        effect_2.gameObject.SetActive(false);
        coll.gameObject.SetActive(false);
    }
}
