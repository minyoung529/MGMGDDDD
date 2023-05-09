using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TogglePosition))]
public class SlidingDoor : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] int needKeyCount = 1;
    [SerializeField] bool isClose = false;
    [SerializeField] bool isLock = false;
    [SerializeField] UnityEvent unlockEvent;

    private bool open = false;
    private int inputKeyCount = 0;
    
    TogglePosition togglePos;

    private void Awake()
    {
        togglePos= GetComponent<TogglePosition>();
    }

    public void Open()
    {
        if(isLock) return;
        open = true;
        togglePos.Open();
    }

    public void Close()
    {
        open = false;
        togglePos.Close();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (!open)
            {
                Open();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (open && isClose)
            {
                Open();
            }
        }
    }

    public void Lock()
    {
        isLock= true;
    }
    public void UnLock()
    {
        isLock= false;
    }

    public void InputKey(Key key)
    {
        inputKeyCount++;

        if (inputKeyCount >= needKeyCount)
        {
            unlockEvent?.Invoke();
            UnLock();
        }
    }
}
