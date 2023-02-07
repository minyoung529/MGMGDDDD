using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelting : MonoBehaviour
{
    //  [SerializeField] GameObject bridge;

    [SerializeField] private bool inObj = false;
    private bool melting = false;
    private Rigidbody inObjRigid;
    private Collider inObjCollider;

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

    public void IceMelt()
    {
        if(inObj)
        {
            inObjRigid = transform.GetChild(0).GetComponent<Rigidbody>();
            inObjCollider = transform.GetChild(0).GetComponent<Collider>();
            transform.GetChild(0).SetParent(null);
            inObjCollider.enabled = true;
        }
        
        transform.DOScaleY(0f, 1.9f).OnComplete(() =>
        {
            if(inObj)
            {
                inObjRigid.isKinematic = false;
                inObjRigid.useGravity = true;
            }
            //bridge.transform.DOScaleZ(15f, 1f);
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
