using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] bool isFirst = true;
    [SerializeField] bool isRotate = false;
    [SerializeField] bool isOil = false;
    float rotSpeed = 1.0f;


    public void OnGear()
    {
        if (!isOil) return;
        isRotate = true;
        StartCoroutine(RotateGear());
    }
    public void StopGear()
    {
        isRotate = false;
        StopCoroutine(RotateGear());
    }


    IEnumerator RotateGear()
    {
        while (isRotate)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(new Vector3(0, 0, 1.2f));
            if (isFirst)
            {
                if (transform.GetChild(0).position.y <= transform.position.y)
                {
                    AppearSticky();
                }
            }
        }
    }



    private void AppearSticky()
    {
        Pet pet = transform.GetChild(0).GetComponent<Pet>();
        if (pet != null)
        {
            pet.AppearPet();
            transform.GetChild(0).SetParent(null);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("OilBullet"))
        {
            if (isRotate || !isFirst) return;
            isOil = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OilBullet"))
        {
            if (isRotate || !isFirst) return;
            isOil = true;
        }
    }
}
