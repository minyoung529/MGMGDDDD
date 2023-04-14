using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialPuzzleController : MonoBehaviour
{
    [SerializeField] private float lessTimer = 30f;
    [SerializeField] private string hintString = "";
    [SerializeField] private GameObject map;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<AnswerData> datas = new List<AnswerData>();

    public string Hint => hintString;
    public TimeType CurState => curState;

    private bool pause = false;
    private float remainTime = 0;
    private Coroutine timerCoroutine;



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

            remainTime -= Time.deltaTime;
            yield return null;
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