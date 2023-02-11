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
            Vector3 targetpos = Vector3.MoveTowards(movedTransform.position, target.position, Time.deltaTime);
            if (rigid)
            {
                rigid.MovePosition(targetpos);
            }
            else
            {
                movedTransform.position = targetpos;
            }
        }
    }
}
