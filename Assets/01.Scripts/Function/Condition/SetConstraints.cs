using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConstraints : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private Vector3 rotation;

    [SerializeField]
    private RigidbodyConstraints constraints;

    private Rigidbody rigid;

    private bool isRotating = false;

    private void Start()
    {
        rigid = Utils.GetOrAddComponent<Rigidbody>(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isRotating) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            Debug.Log("Sdf");
            isRotating = true;
            transform.DORotate(rotation, 0.5f).OnComplete(() => isRotating = false);
            rigid.constraints = constraints;
        }
    }
}
