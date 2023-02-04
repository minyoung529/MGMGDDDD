using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelting : MonoBehaviour
{
    private bool melting = false;

    private float meltReadyTime = 3.0f;

    private IEnumerator StartMelt()
    {
        melting = true;
        yield return new WaitForSeconds(meltReadyTime);
        if (melting)
        {
            IceMelt();
        }
    }

    private void IceMelt()
    {
        Rigidbody rb = transform.GetChild(0).GetComponent<Rigidbody>();
        transform.GetChild(0).SetParent(null);

        transform.DOScaleY(0f, 1.9f).OnComplete(() =>
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        });
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fire fire = collision.collider.GetComponent<Fire>();
        if (fire != null)
        {
            if (fire.IsBurn)
            {
                StartCoroutine(StartMelt());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Fire fire = collision.collider.GetComponent<Fire>();
        if (fire != null)
        {
            if (fire.IsBurn && melting)
            {
                melting = false;
                StopCoroutine(StartMelt());
            }
        }
    }
}
