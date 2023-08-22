using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpringObject : HoldableObject
{
    public override void OnDrop()
    {
        collider.enabled = true;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
    }

    public override void OnHold()
    {
        isHold = true;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;
        collider.enabled = false;
    }

    public override void OnDropFinish()
    {
        isHold = false;
    }

    public override void OnLanding()
    {
    }

    public void OnClear()
    {
        Destroy(gameObject);
    }
}
