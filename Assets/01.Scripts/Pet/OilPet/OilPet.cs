using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;

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
    protected override void ActiveSkill(InputAction inputAction, float value)
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsCoolTime) return;
        base.ActiveSkill(inputAction, value);

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = CreateOil();

            Vector3 dir = (hit.point - transform.position) + (Vector3.up*1.3f);
            dir.y = 0;
            oil.transform.DOScale(oil.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
            oil.transform.DOMoveY(hit.point.y, 2f).SetEase(Ease.OutQuad);
            oil.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        }
    }
    private GameObject CreateOil()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        OilPaint oil = Instantiate(oilSkill, spawnPoint, Quaternion.identity).GetComponent<OilPaint>();
        return oil.gameObject;
    }

    #endregion

}
