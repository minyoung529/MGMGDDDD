using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractOilObject))]
public class UnMovedObject : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float mass = 30f;

    private float moveDistance = 0f;
    private Vector3 prevDistacne;

    private bool calcDistance = true;
    private RigidbodyConstraints originalConstraints;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.mass = 150;
        originalConstraints = rigid.constraints;
        prevDistacne = transform.position;
    }

    private void Update()
    {
        if (!calcDistance) return;
        CalcDistance();

        if (moveDistance > 2f)
        {
            StopMoving();
        }
    }

    public void OnEnterOil()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.ResetCenterOfMass();
        rigid.ResetInertiaTensor();
        rigid.constraints = originalConstraints;
        rigid.mass = mass;
        calcDistance = false;
    }

    private void StopMoving()
    {
        calcDistance = false;
        rigid.constraints = RigidbodyConstraints.FreezeAll;

        // 이때 무슨 소리 
    }

    private void CalcDistance()
    {
        moveDistance += Vector3.Distance(prevDistacne, transform.position);
        prevDistacne = transform.position;
    }
}
