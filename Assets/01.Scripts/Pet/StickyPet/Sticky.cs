using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private bool isSticky = false;
    public bool IsSticky { get { return isSticky; } set { isSticky = value; } }

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public void SetSticky()
    {
        if(isSticky) { return; }

        isSticky = true;
        rigid.useGravity = false;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;
    }
}
