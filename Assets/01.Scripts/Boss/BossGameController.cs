using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossGameController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onClear;


    [SerializeField]
    private UnityEvent onFail;

    public void Clear()
    {
        Debug.Log("Clear");
        onClear?.Invoke();
    }

    public void Fail()
    {
        onFail?.Invoke();
    }
}
