using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleClearCounter : MonoBehaviour
{
    [SerializeField] private int clearCount = 1;
    [SerializeField] UnityEvent onClear;

    private int count = 0;

    private void Awake()
    {
        count = 0;
    }

    public void CheckClear()
    {
        count++;
        if(count == clearCount)
        {
            Clear();
        }
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        onClear?.Invoke();
    }
}
