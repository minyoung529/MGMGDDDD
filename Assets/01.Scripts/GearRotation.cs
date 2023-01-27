using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    float rotSpeed = 1.0f;
    bool isRotate = false;

    public void OnGear()
    {
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
        while(isRotate)
        {
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(new Vector3(0, 1, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Finish"))
        {
            if (isRotate) return;
            OnGear();
        }
    }

}
