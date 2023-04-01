using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveChildTogglePosition : MonoBehaviour
{
    private TogglePosition[] togglePositions;

    private void Start()
    {
        togglePositions = GetComponentsInChildren<TogglePosition>();
    }

    public void Trigger()
    {
        foreach (var position in togglePositions)
        {
            position.Trigger();
        }
    }

    public void Open()
    {
        foreach (var position in togglePositions)
        {
            position.Open();
        }
    }

    public void Close()
    {
        foreach (var position in togglePositions)
        {
            position.Close();
        }
    }
}
