using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToOriginal : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    private bool isContact = false;

    private void Update()
    {
        if (isContact) return;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((collision.gameObject.layer << 1) & layerMask) != 0)
        {
            isContact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((collision.gameObject.layer << 1) & layerMask) != 0)
        {
            isContact = false;
        }
    }
}
