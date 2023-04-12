using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro.Examples;
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

    // 여기서 Null 오류가 난다면 07.Prefab/Puzzle/Dial에 Spawn Points를 씬으로 끌고 와서
    // 그 자식들을 DialPuzzleController의 SpawnPoints로 옮겨주시면 됩니다.
    public Vector3 SpawnPosition => spawnPoints[(int)curState].position;

    private bool pause = false;
    private float remainTime = 0;
    private Coroutine timerCoroutine;

    private void Start()
    {
        StartDialPuzzle();
    }

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Pet.IsCameraAimPoint = false;
        OilPetSkill.IsCrosshair = false;
    }

    public void StopDialPuzzle()
    {
        StopTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = true;
        OilPetSkill.IsCrosshair = true;
    }
    #endregion
}