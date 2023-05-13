using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVisual : MonoBehaviour
{
    public void Trigger()
    {
        OnTrigger();
    }

    protected virtual void OnTrigger() { }
}
