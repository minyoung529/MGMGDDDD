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
/// <summary>
/// ����
/// 1. ������ ���� ��ȣ�ۿ� F�� �����ϰ� �Ѵ�.
/// 2. ��ȣ�ۿ� ��
///    - ���� ȸ��
///    - �̺�Ʈ ����
/// 3. �ݴ�� �ٽ� ��ȣ�ۿ� ��
///    - ���� �ݴ�� ȸ��
///    - �̺�Ʈ ����
/// </summary>
public class Lever : MonoBehaviour
{
    [Header("������ ON������ ����� ��")]
    public UnityEvent OnLever;
    [Header("������ OFF������ ����� ��")]
    public UnityEvent OffLever;
    public LayerMask playerLayer;

    private Transform handle;
    private bool ice = false;
    private bool isNear = false;
    private bool toggle = false;

    private float nearRadius = 0.8f;

    private void Start()
    {
        SetLever();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (NearPlayer()) ToggleEvent();
        }
    }

    private void SetLever()
    {
        // �����
       //OnLever.AddListener(DebugOnLever);
       //OffLever.AddListener(DebugOffLever);

        // �ʼ�
        handle = transform.GetChild(0);
    }

    #region Boolean
    // ��ȣ�ۿ� ������ �����ΰ� üũ�ϴ� �Լ�
    private bool NearPlayer()
    {
        if (isNear) return true;
        return false;
    }
#endregion

    #region Event
    // Event Toggle
    private void ToggleEvent()
    {
        toggle = !toggle;
        if(toggle)
        {
            EventStart();
        }
        else
        {
            EventStop();
        }
    }

    // �̺�Ʈ ����
    private void EventStart()
    {
        OnRotateLever();
        OnLever.Invoke();
    }
    // �̺�Ʈ ����
    private void EventStop()
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
        if(other.CompareTag("Player"))
        {
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}
