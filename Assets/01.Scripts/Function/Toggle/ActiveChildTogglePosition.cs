using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveChildTogglePosition : MonoBehaviour
{
    private TogglePosition[] togglePositions;

    private void Awake()
    {
        togglePositions = GetComponentsInChildren<TogglePosition>();
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        foreach (var position in togglePositions)
        {
            position.Trigger();
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        foreach (var position in togglePositions)
        {
            position.Open();
        }
    }

    [ContextMenu("Close")]
    public void Close()
    {
        foreach (var position in togglePositions)
        {
            position.Close();
        }
    }

    public void ForceClosePos()
    {
        foreach (var position in togglePositions)
        {
            position.ForceClosePosition();
        }

    }

    public void ForceOpenPos()
    {
        Debug.Log("Open");
        foreach (var position in togglePositions)
        {
            position.ForceOpenPosition();
        }
    }
}
