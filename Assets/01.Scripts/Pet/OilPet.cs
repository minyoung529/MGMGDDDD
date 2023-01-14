using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPet : Pet
{
    [SerializeField] PhysicMaterial oilPhysic;
    private const float fireStayTime = 5.0f;
    private const float oilSkillRadius = 10.0f;
    private bool isFire = false;

    protected override void ResetPet()
    {
        base.ResetPet();

        isFire = false;
    }

    // OilSkill
    // 원하는 곳에 기름을 뿌려 미끄럼게 함
    protected override void Skill()
    {
        base.Skill();
        Debug.Log(gameObject.name + " : Skill");

        Vector3 pos = Vector3.zero;
        Collider[] cols = Physics.OverlapSphere(pos, oilSkillRadius);
        for(int i=0;i<cols.Length;i++)
        {
            // static friction
            cols[i].material = oilPhysic;
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

    private void OnTriggerEnter(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            Debug.Log("Oil_Enter");
            other.material = oilPhysic;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oil_Exit");
            other.material = null;
        }
    }


}
