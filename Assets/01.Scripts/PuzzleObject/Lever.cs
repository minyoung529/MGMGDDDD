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
/// 레버
/// 1. 가까이 가면 상호작용 F가 가능하게 한다.
/// 2. 상호작용 시
///    - 레버 회전
///    - 이벤트 실행
/// 3. 반대로 다시 상호작용 시
///    - 레버 반대로 회전
///    - 이벤트 종료
/// </summary>
public class Lever : MonoBehaviour
{
    [Header("레버를 ON쪽으로 당겼을 때")]
    public UnityEvent OnLever;
    [Header("레버를 OFF쪽으로 당겼을 때")]
    public UnityEvent OffLever;
    public LayerMask playerLayer;

    private Transform handle;
    private bool toggle = false;
    private float nearRadius = 1f;

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
        OnLever.AddListener(DebugOnLever);
        OffLever.AddListener(DebugOffLever);

        // 필수
        handle = transform.GetChild(0);
    }

    #region Boolean
    // 상호작용 가능한 범위인가 체크하는 함수
    private bool NearPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, playerLayer);

        if (colliders.Length > 0) return true;
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

    // 이벤트 시작
    private void EventStart()
    {
        OnRotateLever();
        OnLever.Invoke();
    }
    // 이벤트 종료
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
        handle.DORotate(new Vector3(0f, 0f, -45f), 1f);
    }
    private void OffRotateLever()
    {
        handle.DOKill();
        handle.DORotate(new Vector3(0f, 0f, 45f), 1f);
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
}
