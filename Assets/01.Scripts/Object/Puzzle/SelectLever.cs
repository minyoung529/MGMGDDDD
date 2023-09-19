using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectLever : MonoBehaviour
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private Vector3[] controlObjectRotate;

    public UnityEvent firstLever;
    public UnityEvent secondLever;
    public UnityEvent thirdLever;

    private bool isNear = false;
    public bool disposable = true;
    public LayerMask playerLayer;

    private Transform handle;

    int selectIndex = 1;


    private void Start()
    {
        StartListen();
        SetLever();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Interaction, ToggleEvent);
    }

    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Interaction, ToggleEvent);
    }

    protected virtual bool CheckLever()
    {
        return NearPlayer();
    }

    protected virtual void SetLever()
    {
        handle = transform.GetChild(0);
    }

    #region Boolean
    protected bool NearPlayer()
    {
        if (isNear) return true;
        return false;
    }
    #endregion

    #region Event
    // Event Toggle

    public void ToggleEvent(InputAction action, float value)
    {
        ToggleEvent();
    }

    // Inspector View
    [ContextMenu("Lever")]
    public void ToggleEvent()
    {
        if (!CheckLever()) return;
        if (disposable) return;

        EventStart();
    }

    protected virtual void EventStart()
    {
        selectIndex++;
        if (selectIndex > 2) selectIndex = 0;

        if(selectIndex == 0) firstLever.Invoke();
        if(selectIndex == 1) secondLever.Invoke();
        if(selectIndex == 2) thirdLever.Invoke();

        OnRotateLever();
    }
    #endregion

    private void OnRotateLever()
    {
        handle.DOKill();
        
        float val = 0;
        if (selectIndex == 0) val = -45f;
        else if (selectIndex == 2) val = 45f;
        
        handle.DOLocalRotate(new Vector3(0f, 0f, -val), 1f);
        controlObject.transform.DOLocalRotate(controlObjectRotate[selectIndex], 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.PLAYER_TAG))
        {
            isNear = false;
        }
    }

    private void OnDestroy()
    {
        StopListen();
    }
}
