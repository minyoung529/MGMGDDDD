using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] Transform attachPoint;

    protected bool isGet = false;
    protected bool isNearPlayer = false;
    protected string playerTag = Define.PLAYER_TAG;

    private Rigidbody rigid;
    private PushObject playerPushObj;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isGet = false;
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

    protected virtual void SetEquip(InputAction action, float value)
    {
        isGet = !isGet;

        if (isGet) GetItem();
        else UseItem();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            isNearPlayer = true;
            playerPushObj = other.GetComponent<PushObject>();
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isNearPlayer = false;
            playerPushObj = null;
        }
    }

    protected virtual void GetItem()
    {
        if (!isNearPlayer)
        {
            isGet = false;
            return;
        }
        PickUp();
    }
    public virtual void UseItem()
    {
        Drop();
    }

    public void PickUp()
    {
        if (playerPushObj == null) return;

        playerPushObj.CanPush = true;
        rigid.isKinematic = true;
        rigid.useGravity = false;

        transform.SetParent(attachPoint, true);
        transform.localPosition = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    public void Drop()
    {
        //attachPoint.transform.DetachChildren();
        //playerPushObj.CanPush = false;
        //    playerPushObj = null;
    }

    private void OnDestroy()
    {
        StopListen();
    }
}
