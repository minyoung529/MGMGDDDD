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

    private bool isFail = false;

    private void Awake()
    {
        EventManager.StartListening(EventName.BossFail, Fail);
    }

    public void Clear()
    {
        onClear?.Invoke();
    }

    public void Fail(EventParam param)
    {
        if (isFail)
            return;

        isFail = true;
        onFail?.Invoke();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.BossFail, Fail);
    }
}
