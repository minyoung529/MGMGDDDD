using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HardMoveObject : MonoBehaviour
{
    private Rigidbody rigid;
    private int enterIdx = 0;
    private bool canMove = false;
    private RigidbodyConstraints rigidbodyConstraints;
    public bool CanMove { get { return canMove; } }

    [SerializeField]
    private UnityEvent onMove;

    [SerializeField]
    private UnityEvent onUnMove;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbodyConstraints = rigid.constraints;

        OilPetSkill.OnClearOil += UnMove;
        rigid.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (enterIdx++ == 0)
            {
                Move();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (--enterIdx == 0)
            {
                UnMove();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (enterIdx++ == 0)
            {
                Move();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.OIL_BULLET_TAG))
        {
            if (--enterIdx == 0)
            {
                UnMove();
            }
        }
    }

    private void Move()
    {
        onMove?.Invoke();
        rigid.constraints = rigidbodyConstraints;
        canMove = true;
    }

    public void UnMove()
    {
        onUnMove?.Invoke();

        enterIdx = 0;
        rigid.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
    }

    private void OnDestroy()
    {
        OilPetSkill.OnClearOil -= UnMove;
    }
}