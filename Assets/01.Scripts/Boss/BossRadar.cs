using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossRadar : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<GameObject> onEnter;

    [SerializeField]
    private UnityEvent<GameObject> onExit;

    [SerializeField]
    private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            onEnter?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            onExit?.Invoke(other.gameObject);
        }
    }
}
