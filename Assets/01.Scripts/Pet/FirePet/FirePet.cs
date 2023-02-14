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
            GameObject fireBall = CreateFire();

            Vector3 point = (hit.point - transform.position) + (Vector3.up * 1.3f);
            point.y = 0;
            fireBall.transform.DOMoveY(hit.point.y, 2f).SetEase(Ease.OutQuad);
            fireBall.GetComponent<Rigidbody>().AddForce(point, ForceMode.Impulse);
        }
    }
    private GameObject CreateFire()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
        FireBall fire = Instantiate(fireBall, spawnPoint, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }


    #endregion
}
