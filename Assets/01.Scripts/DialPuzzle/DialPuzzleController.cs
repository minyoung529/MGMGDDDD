using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum TimeType
{
    Morning = 0,
    Afternoon = 1,
    Evening = 2,
    Night = 3,
}

public class DialPuzzleController : MonoBehaviour
{
    [SerializeField] private float lessTimer = 30f;
    [SerializeField] private string hintString = "";
    [SerializeField] private GameObject map;

    private Queue<TimeType> answer = new Queue<TimeType>();
    private TimeType curState = TimeType.Morning;

    public string Hint => hintString;
    public TimeType CurState => curState;


    #region Answer
    public void InputAnswer()
    {
        answer.Enqueue(curState);
    }
    public void OutAnswer()
    {
        answer.Dequeue();
    }
    #endregion

    #region State
    [ContextMenu("ChangeState")]
    public void Test()
    {
        ChangeState((TimeType)((int)curState + 1));
    }
    public void ChangeState(TimeType state)
    {
        Vector3 rot = new Vector3(0, (int)state * 90f, 0);
        map.transform.DORotate(rot, 0.5f).OnComplete(() =>
        {
            CheckRotationState();
        });
    }
    private void CheckRotationState()
    {
        curState = (TimeType)(map.transform.eulerAngles.y/90);
    }

    #endregion

        #region Start/Stop
    public void StartDialPuzzle()
    {

    }
    public void StopDialPuzzle()
    {

    }
    #endregion
}