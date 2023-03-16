using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionReceiver : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onExplosion;

    public void OnExplosion()
    {
        onExplosion?.Invoke();
    }
}
