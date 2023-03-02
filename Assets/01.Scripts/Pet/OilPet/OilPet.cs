using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using Cinemachine;

public class OilPet : Pet
{
    [SerializeField] GameObject oilSkill;
    [SerializeField] GameObject fireBurnParticle;
    [SerializeField] Transform parentController;
    [SerializeField] Transform splatGunNozzle;

    ParticleSystem inkParticle;

    private const float fireStayTime = Define.ICE_MELTING_TIME;
    private const float fireSkillTime = Define.BURN_TIME;
    private const float fireRadius = Define.FIRE_RADIUS;

    private bool isParticleOn;

    protected override void Awake()
    {
        base.Awake();
        inkParticle = parentController.transform.GetComponentInChildren<ParticleSystem>();

    }
    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();
    }
    #endregion

    #region Skill

    // Active skill
    protected override void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = CreateOil();

            Vector3 dir = (hit.point - transform.position) + (Vector3.up * 1.3f);
            dir.y = 0;
            oil.transform.DOScale(oil.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
            oil.transform.DOMoveY(hit.point.y, 2f);
            oil.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);

   //         ParticlePlay(hit.point);
        }
    }
    private void ParticlePlay(Vector3 hit)
    {
        if (isParticleOn) return;
        VisualPolish(hit);
        StartCoroutine(InkParticle());
    }
    IEnumerator InkParticle()
    {
        isParticleOn= true;
            inkParticle.Play();
        yield return new WaitForSeconds(2f);
            inkParticle.Stop();
        isParticleOn= false;
    }
    void VisualPolish(Vector3 point)
    {
        if (!DOTween.IsTweening(parentController))
        {
            parentController.DOComplete();
            Vector3 forward = -parentController.forward;
            Vector3 localPos = parentController.localPosition;
            parentController.DOLocalMove(localPos - new Vector3(0, .2f, 0), .03f)
                .OnComplete(() => parentController.DOLocalMove(localPos, .1f).SetEase(Ease.OutSine));

         //   impulseSource.GenerateImpulse();
        }

        if (!DOTween.IsTweening(splatGunNozzle))
        {
            splatGunNozzle.DOComplete();
            splatGunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);
        }
    }

    private GameObject CreateOil()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        GameObject oil = Instantiate(oilSkill, spawnPoint, Quaternion.identity);
        return oil;
    }

    #endregion

}
