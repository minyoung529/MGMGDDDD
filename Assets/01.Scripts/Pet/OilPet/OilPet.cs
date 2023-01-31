using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilPet : Pet
{
    [SerializeField] GameObject oilSkill;
    [SerializeField] GameObject fireBurnParticle;

    private const float fireStayTime = Define.ICE_MELTING_TIME;
    private const float fireSkillTime = Define.BURN_TIME;
    private const float fireRadius = Define.FIRE_RADIUS;

    private bool isBurn = false;
    private bool inFire = false;

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        isBurn = false;
        inFire = false;
    }
    #endregion

    #region Skill

    // Active skill
    protected override void ClickActive()
    {
        if (!IsSkilling) return;
        base.ClickActive();

        isSkilling = false;
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = CreateOil();

            oil.transform.DOMoveX(hit.point.x, 1).SetEase(Ease.OutQuad);
            oil.transform.DOMoveZ(hit.point.z, 1).SetEase(Ease.OutQuad);
            oil.transform.DOMoveY(hit.point.y, 1).SetEase(Ease.InQuad).OnComplete(()=>
            {
                CoolTime(Define.ACTIVE_COOLTIME_TYPE);
            });
        }
    }
    private GameObject CreateOil()
    {
        GameObject oil = Instantiate(oilSkill, transform.position, Quaternion.identity);
        return oil;
    }

    // Passive Skill
    protected override void PassiveSkill()
    {
        if (IsPassiveCoolTime) return;
        base.PassiveSkill();

        StartCoroutine(FireTime());
    }

    IEnumerator FireTime()
    {
        isBurn = true;
        CoolTime(Define.PASSIVE_COOLTIME_TYPE);
        StartCoroutine(FireSkill());
        yield return new WaitForSeconds(fireSkillTime);
        isBurn = false;
    }
    IEnumerator FireSkill()
    {
        fireBurnParticle.SetActive(true);
        while (true)
        {
            if (!isBurn) break;
            yield return new WaitForSeconds(0.01f);

            Collider[] cols = Physics.OverlapSphere(transform.position, fireRadius);
            for (int i = 0; i < cols.Length; i++)
            {
                Fire fire = cols[i].GetComponent<Fire>();
                if (fire != null)
                {
                    if (fire.IsBurn) continue;
                    Instantiate(fireBurnParticle, fire.transform.position, fire.transform.rotation, fire.transform);
                    fire.Burn();
                }
            }
        }
        fireBurnParticle.SetActive(false);
    }

    private void InFire()
    {
        inFire = true;
        StartCoroutine(FireStayTime());
    }
    private void OutFire()
    {
        inFire = false;
        StopCoroutine(FireStayTime());
    }
    IEnumerator FireStayTime()
    {
        yield return new WaitForSeconds(fireStayTime);

        if (inFire)
        {
            PassiveSkill();
        }
    }

    #endregion

    #region Collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            if (isBurn) return;
            InFire();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            if (isBurn) return;
            OutFire();
        }
    }

    #endregion

}
