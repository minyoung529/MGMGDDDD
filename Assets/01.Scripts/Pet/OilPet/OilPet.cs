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
        if (!IsSkilling || !ThirdPersonCameraControll.IsPetAim) return;
        base.ClickActive();

        isSkilling = false;
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = CreateOil();

            oil.transform.DOMoveX(hit.point.x, 1).SetEase(Ease.OutQuad);
            oil.transform.DOMoveZ(hit.point.z, 1).SetEase(Ease.OutQuad);
            oil.transform.DOMoveY(hit.point.y, 1).SetEase(Ease.InQuad).OnComplete(() =>
            {
                CoolTime();
            });
            //Vector3 point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            //oil.transform.DOMove(point, 1).SetEase(Ease.InQuad).OnComplete(() =>
            //{
            //    CoolTime();
            //});
        }
    }
    private GameObject CreateOil()
    {
        OilPaint oil = Instantiate(oilSkill, transform.position, Quaternion.identity).GetComponent<OilPaint>();
        if (isBurn) oil.SetBurn();
        return oil.gameObject;
    }

    #endregion

}
