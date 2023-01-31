using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    float rotSpeed = 1.0f;
    bool isRotate = false;
    bool isFirst = false;

    private void Awake()
    {
        EventManager.StartListening(EventName.StopGear, StopGear);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.StopGear, StopGear);
    }

    public void OnGear()
    {
        isRotate = true;
        StartCoroutine(RotateGear());
    }
    public void StopGear(Dictionary<string, object> dic)
    {
        isRotate = false;
        StopCoroutine(RotateGear());
    }

    private void Start()
    {
        if (!isFirst) OnGear();
    }

    IEnumerator RotateGear()
    {
        while (isRotate)
        {
            yield return new WaitForSeconds(0.1f);
            transform.Rotate(new Vector3(0, 1, 0));
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
        if (collision.collider.CompareTag("Finish"))
        {
            if (isRotate || !isFirst) return;

            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.constraints ^= RigidbodyConstraints.FreezeRotationY;
            // OnGear();
            // 임시로 막아놓았습니다
        }
    }
}
