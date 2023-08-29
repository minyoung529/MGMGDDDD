using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_Once : TutorialTrigger
{
    private bool keyDown = false;

    protected override void Start()
    {
        base.Start();
        keyDownAction += OnKeyDown;
    }

    protected override bool Condition(Transform player)
    {
        return !keyDown;
    }

    private void OnKeyDown(InputAction action, float value)
    {
        keyDown = true;
    }

    private void OnDestroy()
    {
        keyDownAction -= OnKeyDown;
    }
}
