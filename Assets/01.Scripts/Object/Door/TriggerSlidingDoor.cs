using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TriggerSlidingDoor : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] bool once = false;

    private bool isTrigger = false;
    private TogglePosition[] doors;

    private void Awake()
    {
        doors = GetComponentsInChildren<TogglePosition>();
    }

    public void Trigger()
    {
        if (once && isTrigger) return;

        isTrigger = true;
        foreach (TogglePosition toggle in doors)
        {
            toggle.Trigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Trigger();
        }
    }
}
