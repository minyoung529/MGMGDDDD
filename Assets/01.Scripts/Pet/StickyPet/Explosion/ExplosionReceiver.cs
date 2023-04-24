using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionReceiver : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onExplosion;

    [SerializeField]
    private UnityEvent<ExplosionInfo> onExplosionWithInfo;

    public void OnExplosion(ExplosionInfo info)
    {
        onExplosion?.Invoke();
        onExplosionWithInfo?.Invoke(info);
    }
}
