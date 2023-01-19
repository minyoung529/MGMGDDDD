using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilPet : Pet
{
   // [SerializeField] PhysicMaterial oilPhysic;
    [SerializeField] GameObject oilSkill;

    private const float fireStayTime = 5.0f;

    private bool isFire = false;
    private bool isSkilling = false;

    protected override void ResetPet()
    {
        base.ResetPet();

        isFire = false;
        isSkilling = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isSkilling)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickSkill();
                PetManager.instance.AltPress(false);
            }
        }

    }

    // OilSkill
    // ���ϴ� ���� �⸧�� �ѷ� �̲����� ��
    protected override void Skill()
    {
        base.Skill();
        Debug.Log(gameObject.name + " : Skill");
        isSkilling = true;
    }

    private void ClickSkill()
    {
        Debug.Log("Click_Skill");

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            GameObject oil = Instantiate(oilSkill, transform.position, Quaternion.identity);
            oil.transform.DOMoveX(hit.point.x, 3).SetEase(Ease.OutQuad);
            oil.transform.DOMoveY(hit.point.y, 3).SetEase(Ease.InQuad);
            
            isSkilling = false;
        }
    }

    #region FIRE

    private void FireSkill()
    {
        isFire = true;
        StartCoroutine(FireStayTime());
    }

    IEnumerator FireStayTime()
    {
        yield return new WaitForSeconds(fireStayTime);
        isFire = false;
    }

    #endregion


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.CompareTag("Fire"))
    //    {
    //        FireSkill();
    //    }
    //}



}
