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

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = CreateOil();

            Vector3 dir = (hit.point - transform.position);
            oil.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
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
