using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] bool isRotate = false;
    [SerializeField] bool isOil = false;
    float rotSpeed = 1.0f;

    private void Awake()
    {
        if(isRotate)
        {
            StartGear();
        }
    }
    public void StartGear()
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
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            isOil = true;
            StartGear();
        }
    }
}
