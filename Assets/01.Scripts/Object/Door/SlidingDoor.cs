using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TogglePosition))]
public class SlidingDoor : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] bool isClose = false;
    private bool open = false;

    TogglePosition togglePos;

    private void Awake()
    {
        togglePos= GetComponent<TogglePosition>();
    }

    public void Open()
    {
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

}
