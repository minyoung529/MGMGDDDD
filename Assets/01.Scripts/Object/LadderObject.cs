using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderObject : MonoBehaviour
{
    private static bool isLadder = false;
    public static bool IsLadder => isLadder;
    private PlayerMove movement;

    private void OnCollisionEnter(Collision collision)
    {
        OnEnterLadder(collision.rigidbody);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnExitLadder(collision.rigidbody);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnterLadder(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExitLadder(other.attachedRigidbody);
    }

    private void OnExitLadder(Rigidbody rigid)
    {
        if (rigid.gameObject.layer != Define.PLAYER_LAYER) return;

        movement ??= rigid.GetComponent<PlayerMove>();

        movement.IsDecelerate = false;

        rigid.useGravity = true;
        isLadder = false;
    }

    private void OnEnterLadder(Rigidbody rigid)
    {
        if (rigid.gameObject.layer != Define.PLAYER_LAYER) return;

        movement ??= rigid.GetComponent<PlayerMove>();
        movement.IsDecelerate = true;

        rigid.useGravity = false;
        rigid.velocity = Vector3.zero;
        isLadder = true;
        rigid.transform.forward = -transform.right;
        movement.IsDecelerate = true;
    }
}
