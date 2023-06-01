using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVisual : MonoBehaviour
{
    protected Action onComplete;

    public void Trigger()
    {
        OnTrigger();
    }

    protected virtual void OnTrigger() { }

    public void ListenCompleteEvent(Action action)
    {
        onComplete += action;
    }

    public void RemoveCompleteEvent(Action action)
    {
        onComplete -= action;
    }
}
