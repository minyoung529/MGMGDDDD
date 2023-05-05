using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePet : Pet
{
    [SerializeField] GameObject fireBall;

    Fire fire;

    protected override void Awake()
    {
        base.Awake();
        fire = GetComponent<Fire>();
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        fire.StopBurn();
    }
    #endregion

    #region Skill

    // Active skill
    public override void Skill()
    {
        if (IsCoolTime) return;
        base.Skill();

        StartCoroutine(FireBurn());
    }

    public void OffSkill()
    {
        Skilling = false;
        fire.StopBurn();
    }

    private IEnumerator FireBurn()
    {
        OnSkill();
        yield return new WaitForSeconds(petInform.skillDelayTime);
        OffSkill();
    }

    public void OnSkill()
    {
        if(fire == null)
        {
            fire = GetComponent<Fire>();
        }
        fire.Burn();
    }

    public override void StopSkill()
    {
        base.StopSkill();

        OffSkill();
    }

    private GameObject CreateFire()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        FireBall fire = Instantiate(fireBall, spawnPoint, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }

    #endregion
}
