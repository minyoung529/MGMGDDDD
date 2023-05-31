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

    public void OnListeningCompleteEvent(Action action)
    {
        onComplete += action;
    }

    public void OnRemoveCompleteEvent(Action action)
    {
        onComplete -= action;
    }
}
