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
    private ToggleScale[] doorScale;

    private void Awake()
    {
        doors = GetComponentsInChildren<TogglePosition>();
        doorScale = GetComponentsInChildren<ToggleScale>();
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        if (once && isTrigger) return;

        isTrigger = true;
        foreach (TogglePosition toggle in doors)
        {
            toggle.Trigger();
        }

        foreach (ToggleScale toggle in doorScale)
        {
            toggle.Trigger();
        }
    }
    public void ForceOpen()
    {
        if (once && isTrigger) return;

        isTrigger = true;
        foreach (TogglePosition toggle in doors)
        {
            toggle.ForceOpenPosition();
        }

        foreach (ToggleScale toggle in doorScale)
        {
            toggle.ForceSize();
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
