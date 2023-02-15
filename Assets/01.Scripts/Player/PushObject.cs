using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private Transform leftHand, rightHand;
    private Vector3 leftPos, rightPos;

    Rigidbody pushedRigid;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        leftPos = leftHand.position;
        rightPos = rightHand.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody && ((1 << collision.gameObject.layer) & layer) != 0)
        {
            pushedRigid = collision.rigidbody;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (pushedRigid && collision.rigidbody == pushedRigid)
        {
            pushedRigid = null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (pushedRigid)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            animator.SetIKPosition(AvatarIKGoal.RightHand, leftHand.position);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, rightHand.position);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
}
