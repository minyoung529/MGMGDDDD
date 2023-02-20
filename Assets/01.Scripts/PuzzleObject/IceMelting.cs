using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class IceMelting : MonoBehaviour
{
    [SerializeField] private bool inObj = false;
    [SerializeField] private float meltTime = 3.8f;
    [SerializeField] private UnityEvent OnMeltIce;
    [SerializeField] private GameObject obj;
    private bool melting = false;
    private float meltReadyTime = 3.0f;

    private Rigidbody inObjRigid;
    private Collider inObjCollider;

    private void Awake()
    {
        if (inObj)
        {
            SetIce();
        }
    }

    #region SET

    private void SetIce()
    {
        inObjCollider = obj.GetComponent<Collider>();
        inObjCollider.enabled = false;

        inObjRigid = obj.GetComponent<Rigidbody>();
        inObjRigid.isKinematic = true;
        inObjRigid.useGravity = false;
    }

    #endregion
    public void Melt()
    {
        if (inObj)
        {
            IceMeltInObj();
        }
        else IceMelt();
    }
    private IEnumerator StartMelt()
    {
        melting = true;
        yield return new WaitForSeconds(meltReadyTime);
        if (melting)
        {
            if (inObj)
            {
                IceMeltInObj();
            }
            else IceMelt();
        }
    }

    public void IceMelt()
    {
        OnMeltIce?.Invoke();
        transform.DOScaleY(0f, meltTime);
        Destroy(gameObject, 2f);
    }

    public void IceMeltInObj()
    {
        obj.transform.SetParent(null);
        
        inObjCollider.enabled = true;
        transform.DOScaleY(0f, 1.9f).OnComplete(() =>
        {
            inObjRigid.isKinematic = false;
            inObjRigid.useGravity = true;
        });

        OnMeltIce?.Invoke();

        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fire fire = collision.collider.GetComponent<Fire>();
        if (fire != null)
        {
            if (fire.IsBurn)
            {
                UnityEngine.Debug.Log("Melt");
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
