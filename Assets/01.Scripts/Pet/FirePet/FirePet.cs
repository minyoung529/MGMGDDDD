using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePet : Pet
{
    [SerializeField] GameObject fireBall;

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();
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
            GameObject fireBall = CreateOil();

            //fireBall.transform.DOMoveX(hit.point.x, 1).SetEase(Ease.OutQuad);
            //fireBall.transform.DOMoveZ(hit.point.z, 1).SetEase(Ease.OutQuad);
            //fireBall.transform.DOMoveY(hit.point.y, 1).SetEase(Ease.InQuad).OnComplete(() =>
            //{
            //    CoolTime();
            //});

            fireBall.transform.DOMove(hit.point, 1f).OnComplete(() =>
            {
                CoolTime();
            });
        }
    }
    private GameObject CreateOil()
    {
        FireBall fire = Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }


    #endregion
}
