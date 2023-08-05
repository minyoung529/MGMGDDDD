using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectLayer : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private UnityEvent onContact;

    [SerializeField]
    private UnityEvent<Collision> onContactWithCollision;

    [SerializeField]
    private bool isOnce;
    private int enterCount = 0;

    void OnCollisionEnter(Collision other)
    {
        if (isOnce && enterCount > 0) return;
        
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            enterCount++;
            onContact?.Invoke();
            onContactWithCollision?.Invoke(other);
        }
    }
}
