using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] Transform attachPoint;

    public LayerMask playerLayer;
    private float nearRadius = 3f;
    private Rigidbody rigid;
    private Collider collider;

    private bool isGet = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        isGet = false;
    }
    private void Start()
    {
        StartListen();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Interaction, SetEquip);
    }
  private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Interaction, SetEquip);
    }

    #region Boolean
    private bool NearPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, playerLayer);
       return colliders.Length > 0;
    }
    #endregion

    protected virtual void SetEquip(InputAction action, float value)
    {
       if (!NearPlayer()) return;

        isGet = !isGet;
        rigid.isKinematic = isGet;
        collider.enabled = !isGet;

        if (isGet) GetItem();
        else UseItem();
    }
    
    protected virtual void GetItem()
    {
        isGet = true;

        transform.SetParent(attachPoint, true);
        transform.localPosition = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    protected virtual void UseItem()
    {
        isGet = false;

        attachPoint.transform.DetachChildren();
    }

    protected virtual void PutItem()
    {
        isGet = false;

        //rigid.isKinematic = false;
        //rigid.useGravity = true;
        //transform.SetParent(null);
    }
}
