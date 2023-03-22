using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private UnityEvent OnEnter;

    [SerializeField]
    private UnityEvent OnExit;

    [SerializeField]
    private bool isOnce = true;
    private bool isTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOnce && isTrigger) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isTrigger = true;
            OnEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            OnExit?.Invoke();
        }
    }
}
