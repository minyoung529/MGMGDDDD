using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnMovedObject : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float mass = 30f;

    private float moveDistance = 0f;
    private Vector3 prevDistacne;

    private bool calcDistance = true;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.mass = 150;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.ResetCenterOfMass();
            rigid.ResetInertiaTensor();
            rigid.constraints = 0;
            rigid.mass = mass;
            calcDistance = false;
        }
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
