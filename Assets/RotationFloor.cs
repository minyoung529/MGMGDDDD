using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationFloor : MonoBehaviour
{
    Vector3 targetVector = Vector3.zero;

    private void OnCollisionStay(Collision collision)
    {
        targetVector = collision.contacts[0].point - transform.position;
    }
    void Update()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
