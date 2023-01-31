using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderObject : MonoBehaviour
{
    private static bool isLadder = false;
    public static bool IsLadder => isLadder;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigid = collision.rigidbody;
        rigid.useGravity = false;
        isLadder = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.rigidbody.useGravity = true;
        isLadder = false;
    }
}
