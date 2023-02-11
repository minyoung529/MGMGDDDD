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

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject fireBall = CreateOil();

            Vector3 point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            fireBall.transform.DOMove(point, 1f);
        }
    }
    private GameObject CreateOil()
    {
        FireBall fire = Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }


    #endregion
}
