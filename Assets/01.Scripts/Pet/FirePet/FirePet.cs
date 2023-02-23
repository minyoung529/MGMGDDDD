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
    protected override void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);

        //RaycastHit hit;
        //if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        //{
        //    GameObject fireBall = CreateFire();

        //    Vector3 dir = (hit.point - transform.position) + (Vector3.up * 1.3f);
        //    dir.y = 0;

        //    fireBall.transform.DOMoveY(hit.point.y, 1.5f).SetEase(Ease.OutQuad);
        //    fireBall.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        //}
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag(Define.OIL_PET_TAG) || collision.collider.CompareTag(Define.PLAYER_TAG)) return;

    //    Fire[] fires = collision.collider.GetComponents<Fire>();
    //    if (fires.Length > 0)
    //    {
    //        fires[0].Burn();
    //    }

    //    IceMelting[] ices = collision.collider.GetComponents<IceMelting>();
    //    if (ices.Length > 0)
    //    {
    //        ices[0].Melt();
    //    }
    //}

    private GameObject CreateFire()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
        FireBall fire = Instantiate(fireBall, spawnPoint, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }

    #endregion
}
