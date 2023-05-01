using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] int needKeyCount = 1;
    [SerializeField] bool isClose = false;
    [SerializeField] bool isLock = false;
    [SerializeField] Light[] checkLights;
    private bool open = false;
    private int inputKeyCount = 0;

    ToggleRotation[] toggleRotations;

    private void Awake()
    {
        toggleRotations = GetComponentsInChildren<ToggleRotation>();
    }

    public void Open()
    {
        if (isLock) return;
        open = true;

        foreach(ToggleRotation rotation in toggleRotations)
        {
            rotation.Open();
        }
    }

    public void Close()
    {
        open = false;
        foreach (ToggleRotation rotation in toggleRotations)
        {
            rotation.Close();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (!open)
            {
                Open();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            if (!open)
            {
                Debug.Log(gameObject.name + " Open   " +  collision.gameObject.name);
                Open();
            }
        }
    }


    public void Lock()
    {
        isLock = true;
    }
    public void UnLock()
    {
        isLock = false;
    }

    public void InputKey(Key key)
    {
        inputKeyCount++;
        checkLights[key.ActiveIdx].color = key.Color;

        if (inputKeyCount >= needKeyCount)
        {
            UnLock();
        }
    }
}
