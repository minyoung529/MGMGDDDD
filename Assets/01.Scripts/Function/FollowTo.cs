using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTo : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }

    private Transform movedTransform;
    public Transform MovedTransform { get { return movedTransform; } set { movedTransform = value; } }

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        movedTransform = transform;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            if (rigid)
            {
                rigid.MovePosition(target.position);
            }
            else
            {
                movedTransform.position = target.position;
            }
        }
    }
}
