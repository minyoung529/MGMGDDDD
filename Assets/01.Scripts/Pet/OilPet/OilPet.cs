using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilPet : Pet
{
    [SerializeField] GameObject oilSkill;

    private const float fireStayTime = 2.0f;
    private const float fireSkillTime = 10.0f;
    private const float fireRadius = 5.0f;

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
        base.ClickActive();

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = Instantiate(oilSkill, transform.position, Quaternion.identity);
            oil.transform.DOMoveX(hit.point.x, 3).SetEase(Ease.OutQuad);
            oil.transform.DOMoveY(hit.point.y, 3).SetEase(Ease.InQuad);

            IsSkilling = false;
        }
    }

    // Passive Skill
    protected override void PassiveSkill()
    {
        base.PassiveSkill();

        Debug.Log("Passive Skill On : 활활 타임 시작");
        isBurn = true;
        StartCoroutine(FireTime());
    }

    IEnumerator FireStayTime()
    {
        yield return new WaitForSeconds(fireStayTime);

        if (inFire)
        {
            PassiveSkill();
        }
    }
    IEnumerator FireTime()
    {
        StartCoroutine(FireSkill());
        yield return new WaitForSeconds(fireSkillTime);
        isBurn = false;
    }
    IEnumerator FireSkill()
    {
        Debug.Log("활활이 시작");
        while (true)
        {
            if (!isBurn) break;

            yield return new WaitForSeconds(0.01f);
            Collider[] cols = Physics.OverlapSphere(transform.position, fireRadius, Define.PET_LAYER | Define.PLAYER_LAYER);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i] != null)
                {
                    // cols[i].Fire();
                }
            }
        }
        Debug.Log("활활이 끝");
    }

    private void InFire()
    {
        Debug.Log("불 : 2초 카운트 시작");
        inFire = true;
        StartCoroutine(FireStayTime());
    }
    private void OutFire()
    {
        Debug.Log("불 : 2초 되기 전에 나감");
        inFire = false;
        StopCoroutine(FireStayTime());
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
