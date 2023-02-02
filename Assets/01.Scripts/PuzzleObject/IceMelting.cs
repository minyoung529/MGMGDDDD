using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelting : MonoBehaviour
{

    public void IceMelt()
    {
        Rigidbody rb = transform.GetChild(0).GetComponent<Rigidbody>();
        transform.GetChild(0).SetParent(null);

        transform.DOScaleY(0f, 2f).OnComplete(() =>
        {
        rb.isKinematic = false;
            rb.useGravity = true;
        });
        Destroy(gameObject, 2f);
    }
}
