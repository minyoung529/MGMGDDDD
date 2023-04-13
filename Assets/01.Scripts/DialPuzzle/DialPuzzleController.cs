using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;

public enum TimeType
{
    Morning = 0,
    Afternoon = 1,
    Evening = 2,
    Night = 3,

    Count
}

[System.Serializable]
public class InputTimeAnswer
{
    public List<TimeType> answerTimeList;
}
public class DialPuzzleController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera fullCam;
    [SerializeField] private CinemachineVirtualCamera defaultCam;

    [SerializeField] private float lessTimer = 30f;
    [SerializeField] private float lessSpeed = 1.5f;
    [SerializeField] private GameObject look;
    [SerializeField] private Transform[] spawnPoints;


    private TimeType curState = TimeType.Morning;

    [SerializeField] TextMeshProUGUI hintText;

    private int round = 0;
    public string Hint => hintString[round];
    private List<TimeType> answer = new List<TimeType>();

    [Header("Dial Answer")]
    [SerializeField] private int maxRound = 2;
    [SerializeField] private List<string> hintString = new List<string>();
    [SerializeField] private List<InputTimeAnswer> correctAnswer = new List<InputTimeAnswer>();
    
    public TimeType CurState => curState;

    public Vector3 SpawnPosition => spawnPoints[(int)curState].position;

    private bool pause = false;
    private float remainTime = 0;
    public float RemainTime { get { return remainTime; } }
    private float rotateSpeed = 10f;
    private Coroutine timerCoroutine = null;

    private bool isFull = false;

    private void Start()
    {
        StartDialPuzzle();
    }

    #region Answer
    [ContextMenu("InputAnswer")]
    public void InputAnswer()
    {
        CheckRotationState();
        answer.Add(curState);
        CheckAnswer();
    }
    public void OutAnswer()
    {
        answer.RemoveAt(answer.Count - 1);
    }

    //private void CheckAnswer()
    //{
    //    if(answer.Count != correctAnswer.Count)
    //    {
    //        Wrong();
    //        return;
    //    }
    //    for(int i=0;i<answer.Count;i++)
    //    {
    //        if (answer[i] != correctAnswer[i])
    //        {
    //            Wrong();
    //            return;
    //        }
    //    }
    //    Correct();
    //}
    private void CheckAnswer()
    {
        if (answer[answer.Count - 1] != correctAnswer[round].answerTimeList[answer.Count - 1])
        {
            Wrong();
        }
        else
        {
            if (answer.Count == correctAnswer[round].answerTimeList.Count) Correct();
        }
    }

    private void Wrong()
    {
        Debug.Log("Answer : " + answer[answer.Count-1] + "Correct : " + correctAnswer[answer.Count-1] + " => Wrong");
    }
    private void Correct()
    {
        answer.Clear();
        
        round++;
        if(round > maxRound)
        {
            Debug.Log("Game End");
            return;
        }
    }

    #region State

    private void CheckRotationState()
    {
        curState = (TimeType)(look.transform.eulerAngles.y / 90);
    }

    #endregion

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

    #region UI

    private void SetHint()
    {
        hintText.SetText(hintString[round]);
    }
    private void SetHint(string str)
    {
        hintText.SetText(str);
    }

    #endregion

    #region Action
    private void RotateMap(InputAction act, float value)
    {
        float dir = act == InputAction.Move_Right ? 1f : -1f;
        look.transform.Rotate(0.0f, dir * Time.deltaTime * rotateSpeed, 0.0f);
        CheckRotationState();
    }

    private void SwitchCam(InputAction act, float value)
    {
        isFull = !isFull;
        if (isFull) CameraSwitcher.SwitchCamera(fullCam);
        else CameraSwitcher.SwitchCamera(defaultCam);
    }
    #endregion

    #region Start/Stop

    private void ResetDial()
    {
        SetHint();
        StartTimer();
        round = 0;
    }

    public void StartDialPuzzle()
    {
        ResetDial();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Pet.IsCameraAimPoint = false;
        OilPetSkill.IsCrosshair = false;

        InputManager.StartListeningInput(InputAction.Zoom, SwitchCam);
        InputManager.StartListeningInput(InputAction.Move_Left, RotateMap);
        InputManager.StartListeningInput(InputAction.Move_Right, RotateMap);
    }


    public void StopDialPuzzle()
    {
        StopTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = true;
        OilPetSkill.IsCrosshair = true;

        InputManager.StopListeningInput(InputAction.Zoom, SwitchCam);
        InputManager.StopListeningInput(InputAction.Move_Right, RotateMap);
        InputManager.StopListeningInput(InputAction.Move_Left, RotateMap);
    }
    #endregion

    private void OnDestroy()
    {
        StopDialPuzzle();
    }
}