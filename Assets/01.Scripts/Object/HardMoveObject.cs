using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardMoveObject : MonoBehaviour
{
    private Rigidbody rigid;
    private int enterIdx = 0;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
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

    private void Move()
    {
        rigid.mass = 1;
        //rigid.drag = 0;
    }

    private void UnMove()
    {
        rigid.mass = 500;
        //rigid.drag = 50;
    }
}
