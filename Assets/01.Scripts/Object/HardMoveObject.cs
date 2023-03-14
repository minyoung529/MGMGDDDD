using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HardMoveObject : MonoBehaviour
{
    [SerializeField] private bool canMove = false;
    public bool CanMove {  get { return canMove; } }
    
    private Rigidbody rigid;
    private int enterIdx = 0;
    private bool inOil = false;

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
                inOil = true;
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
                inOil = false;
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

    public void Move()
    {
        canMove = true;
    }

    public void UnMove()
    {
        canMove = false;
    }
}
