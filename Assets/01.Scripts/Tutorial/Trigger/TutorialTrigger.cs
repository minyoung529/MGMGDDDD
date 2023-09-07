using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private bool isCollide = true;

    [SerializeField, Tooltip("시간이 지나면 자동으로 없어지는지")]
    private bool autoEnd = true;

    [SerializeField, Tooltip("콜라이더가 나가면 자동으로 없어지는지")]
    private bool exitEqualsUnShow = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    private TutorialType tutorialType;

    [SerializeField]
    private string tutorialName;

    private TutorialController tutorialController = null;
    private bool isEnter = false;
    private bool isEnableTutorial = false;
    public bool IsEnableTutorial => isEnableTutorial;

    private Action onEnter;
    private Action onExit;

    #region ACTION

    [Tooltip("이벤트를 구독하지 않으면 굳이 설정할 필요가 없음")]
    [SerializeField]
    private InputAction inputAction = InputAction.Interaction;

    [SerializeField]
    private UnityEvent onKeyDownEvent;

    protected Action<InputAction, float> keyDownAction;

    #endregion
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();

        keyDownAction += (InputAction x, float y) => onKeyDownEvent?.Invoke();

        onEnter += OnEnter;
        onEnter += ListeningEvent;
        onExit += OnExit;
        onExit += StopListeningEvent;
    }

    protected virtual void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide || isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            isEnter = true;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            StartTutorial();
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoEnd || !isEnter) return;
        if (!exitEqualsUnShow) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isEnter = false;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            EndTutorial();
            onExit?.Invoke();
        }
    }

    public void StartTutorial()
    {
        isEnableTutorial = true;
        tutorialController ??= FindObjectOfType<TutorialController>();
        tutorialController?.StartTutorial(tutorialType, onExit, tutorialName);
    }

    public void EndTutorial()
    {
        isEnableTutorial = false;
        tutorialController ??= FindObjectOfType<TutorialController>();
        tutorialController?.StopTutorial(tutorialType);
        StopListeningEvent();
    }

    public void Trigger()
    {
        TutorialController controller = ReadyTutorialStart();

        if (controller)
        {
            StartTutorial(controller);
        }
    }

    public void DelayTrigger(float preDelay)
    {
        TutorialController controller = ReadyTutorialStart();
        if (!controller) return;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(preDelay);
        seq.AppendCallback(() => StartTutorial(controller));
    }

    private TutorialController ReadyTutorialStart()
    {
        TutorialController controller = FindObjectOfType<TutorialController>();
        if (!controller) return null;

        if (Condition(controller.transform)) // player
        {
            if (controller)
            {
                return controller;
            }
        }

        return null;
    }

    private void StartTutorial(TutorialController controller)
    {
        controller.StartTutorial(tutorialType, onExit, tutorialName);
        OnStartTrigger();
    }

    protected virtual bool Condition(Transform player)
    {
        return true;
    }

    protected virtual void OnStartTrigger()
    {

    }

    protected virtual void OnEnter() { }

    protected virtual void OnExit() { }

    private void ListeningEvent()
    {
        if (keyDownAction != null)
        {
            InputManager.StopListeningInput(inputAction, keyDownAction);
            InputManager.StartListeningInput(inputAction, keyDownAction);
        }
    }

    public void StopListeningEvent()
    {
        if (keyDownAction != null)
        {
            InputManager.StopListeningInput(inputAction, keyDownAction);
        }
    }

    public void Inactive()
    {
        onExit?.Invoke();
        if (isCollide) col.enabled = false;
    }

    public void Active()
    {
        ListeningEvent();
        if (isCollide) col.enabled = true;
    }

    #region  SET
    public void SetAutoEnd(bool value) => autoEnd = value;

    public void SetTutorialName(string name) => tutorialName = name;

    public void SetTutorialType(TutorialType type) => tutorialType = type;
    #endregion
}
