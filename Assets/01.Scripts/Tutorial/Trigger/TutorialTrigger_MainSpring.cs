using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger_MainSpring : TutorialTrigger
{
    [SerializeField]
    private HoldableObject obj;

    protected override void Start()
    {
        keyDownAction += OnPutSpring;
    }

    protected override bool Condition(Transform player)
    {
        return obj.IsHold;
    }
    
    private void OnPutSpring(InputAction action, float value)
    {
        Destroy(obj.gameObject);
    }
}
