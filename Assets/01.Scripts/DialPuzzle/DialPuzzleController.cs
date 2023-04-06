using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeType
{
    None = 0,

    Morning = 1,
    Afternoon = 2,
    Evening = 3,
    Night = 4,
}

public class DialPuzzleController : MonoBehaviour
{
    [SerializeField] private float lessTimer = 30f;
    [SerializeField] private string hintString = "";
    [SerializeField] private GameObject map;

    private Queue<TimeType> answer = new Queue<TimeType>();
    private TimeType curState = TimeType.None;

    public string Hint => hintString;
    public TimeType CurState => curState;

    #region Answer
    public void InputAnswer(TimeType ans)
    {
        answer.Enqueue(ans);
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
        ChangeState(TimeType.Evening);
        Debug.Log((TimeType)((int)curState + 1));
    }
    public void ChangeState(TimeType state)
    {
        Debug.Log((int)state);
        Vector3 rot = new Vector3(0, (int)state * 90f, 0);
        map.transform.DORotate(rot, 0.5f).OnComplete(() =>
            {
                CheckCurrentState();
            });

    }
    private void CheckCurrentState()
    {
        double stateValue = Math.Truncate(map.transform.rotation.y / 90);
        Debug.Log(stateValue);
        curState = (TimeType)((int)stateValue);
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