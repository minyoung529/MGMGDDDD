using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilPet : Pet
{
    [SerializeField] GameObject oilSkill;

    private const float fireStayTime = 10.0f;

    private bool isFire = false;
    private bool isSkilling = false;

    private Material mat;

    protected override void Awake()
    {
        base.Awake();

        mat = GetComponent<MeshRenderer>().material;
    }

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
        mat.color = Color.red;
        StartCoroutine(FireStayTime());
    }

    IEnumerator FireStayTime()
    {
        yield return new WaitForSeconds(fireStayTime);
        mat.color = Color.yellow;
        isFire = false;
    }

    #endregion


    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            FireSkill();
        }
    }


}
