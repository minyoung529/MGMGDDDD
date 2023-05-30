using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotAirController : MonoBehaviour
{
    [SerializeField]
    private HotAirBalloon[] balloons;

    private bool[] clearChecker;

    [SerializeField]
    private UnityEvent onClear;

    [SerializeField]
    private UnityEvent onReset;

    void Awake()
    {
        clearChecker = new bool[balloons.Length];
    }

    public void SetIndex(int cubeIdx, int puzzleIdx)
    {
        balloons[cubeIdx].MoveCubeToThis(puzzleIdx);

        if (cubeIdx == puzzleIdx)
        {
            clearChecker[cubeIdx] = true;

            if (CheckClear())
            {
                Clear();
            }
        }
    }

    public void OnReset(int cubeIdx)
    {
        clearChecker[cubeIdx] = false;
        balloons[cubeIdx].OnReset();
        onReset?.Invoke();
    }

    private void Clear()
    {
        onClear?.Invoke();
    }

    private bool CheckClear()
    {
        foreach (bool b in clearChecker)
        {
            if (!b)
                return false;
        }

        return true;
    }
}
