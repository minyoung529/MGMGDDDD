using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractOilObject))]
public class UnMovedObject : MonoBehaviour
{
    private Rigidbody rigid;
     private float mass;

    private float moveDistance = 0f;
    private Vector3 prevDistacne;

    private bool calcDistance = true;
    private RigidbodyConstraints originalConstraints;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        mass = rigid.mass;
        rigid.mass = 230;
        originalConstraints = rigid.constraints;
        prevDistacne = transform.position;
    }

    private void Update()
    {
        if (!calcDistance) return;
        if (!rigid) return;

        CalcDistance();

        if (moveDistance > 2f)
        {
            StopMoving();
        }
    }

    public void OnEnterOil()
    {
        if (!rigid) return;

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.ResetCenterOfMass();
        rigid.ResetInertiaTensor();
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = originalConstraints;
        rigid.mass = mass;
        calcDistance = false;
    }

    private void StopMoving()
    {
        if (!rigid) return;

        calcDistance = false;
        rigid.constraints = RigidbodyConstraints.FreezeAll;

        // ??? ???? ??? 
    }

    private void CalcDistance()
    {
        moveDistance += Vector3.Distance(prevDistacne, transform.position);
        prevDistacne = transform.position;
    }
}
