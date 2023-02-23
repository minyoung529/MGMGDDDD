using DG.Tweening;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Lever : MonoBehaviour
{
    public UnityEvent OnLever;
    public UnityEvent OffLever;
    public bool disposable = true;
    public LayerMask playerLayer;

    private Transform handle;

    private bool isNear = false;
    private bool toggle = false;

    private void Start()
    {
        SetLever();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CheckLever())
            {
                ToggleEvent();
            }
        }
    }

    protected virtual bool CheckLever()
    {
        return NearPlayer();
    }

    protected virtual void SetLever()
    {
        // ?????
       //OnLever.AddListener(DebugOnLever);
       //OffLever.AddListener(DebugOffLever);

        // ???
        handle = transform.GetChild(0);
    }

    #region Boolean
    // ?????? ?????? ??????? u???? ???
    protected bool NearPlayer()
    {
        if (isNear) return true;
        return false;
    }
    #endregion

    #region Event
    // Event Toggle
    [ContextMenu("Lever")]

    private void ToggleEvent()
    {
        toggle = !toggle;
        if(disposable) toggle = true;

        if (toggle)
        {
            EventStart();
        }
        else
        {
            EventStop();
        }
    }

    protected virtual void EventStart()
    {
        OnRotateLever();
        OnLever.Invoke();
    }
    protected virtual void EventStop()
    {
        OffRotateLever();
        OffLever.Invoke();
    }
    #endregion

    #region RotateLever
    private void OnRotateLever()
    {
        handle.DOKill();
        handle.DOLocalRotate(new Vector3(0f, 0f, -45f), 1f);
    }
    private void OffRotateLever()
    {
        handle.DOKill();
        handle.DOLocalRotate(new Vector3(0f, 0f, 45f), 1f);
    }
    #endregion

    #region Debug
    public void DebugOnLever()
    {
        Debug.Log("On Lever");
    }
    public void DebugOffLever()
    {
        Debug.Log("Off Lever");
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = false;
        }
    }
}
