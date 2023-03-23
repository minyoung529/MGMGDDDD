using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardMoveObject : MonoBehaviour
{
    private Rigidbody rigid;
    private int enterIdx = 0;
    private bool canMove  = false;
    private RigidbodyConstraints rigidbodyConstraints;
    public bool CanMove { get { return canMove; }  }

    private float mass = 0f;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbodyConstraints = rigid.constraints;
        mass = rigid.mass;
        UnMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OilBullet"))
        {
            if (enterIdx++ == 0)
            {
                Move();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OilBullet"))
        {
            if (--enterIdx == 0)
            {
                UnMove();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OilBullet"))
        {
            if (enterIdx++ == 0)
            {
                Move();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("OilBullet"))
        {
            if (--enterIdx == 0)
            {
                UnMove();
            }
        }
    }

    private void Move()
    {
        rigid.constraints = rigidbodyConstraints;
        //rigid.drag = drag;
        //rigid.angularDrag = angular;
        canMove = true;
    }

    private void UnMove()
    {
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        //rigid.mass = 100000;
        //rigid.drag = 50000;
        //rigid.angularDrag = 50000;
        canMove = false;
    }
}
