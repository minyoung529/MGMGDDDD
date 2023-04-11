using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public enum TimeType
{
    Morning = 0,
    Afternoon = 1,
    Evening = 2,
    Night = 3,

    Count
}

public class DialPuzzleController : MonoBehaviour
{
    [SerializeField] private float lessTimer = 30f;
    [SerializeField] private float lessSpeed = 1.5f;
    [SerializeField] private string hintString = "";
    [SerializeField] private GameObject map;
    [SerializeField] private Transform[] spawnPoints;

    private Queue<TimeType> answer = new Queue<TimeType>();
    private TimeType curState = TimeType.Morning;

    public string Hint => hintString;
    public TimeType CurState => curState;
    public Vector3 SpawnPosition => spawnPoints[(int)curState].position;

    private bool pause = false;
    private float remainTime = 0;
    private Coroutine timerCoroutine;

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

    #region Timer
    public void StartTimer()
    {
        ClearTimer();
        timerCoroutine = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (remainTime >= 0)
        {
            while (pause)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f * lessSpeed);
            remainTime--;
        }
    }

    private void SetPause(bool _pause)
    {
        pause = _pause;
    }

    private void ClearTimer()
    {
        pause = false;
        remainTime = lessTimer;
    }
    private void StopTimer()
    {
        ClearTimer();
        StopCoroutine(GameTimer());
    }

    #endregion

    #region Start/Stop
    public void StartDialPuzzle()
    {
        StartTimer();
    }

    public void StopDialPuzzle()
    {
        StopTimer();
    }
    #endregion
}