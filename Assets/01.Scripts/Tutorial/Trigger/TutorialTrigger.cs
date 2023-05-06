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

    [SerializeField]
    private bool autoEnd = true;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    private TutorialType tutorialType;

    [SerializeField]
    private string tutorialName;

    private TutorialController tutorialController = null;
    private bool isEnter = false;

    private Action onEnter;
    private Action onExit;

    #region ACTION
    [Tooltip("이벤트를 구독하지 않으면 굳이 설정할 필요가 없음")]
    [SerializeField]
    private InputAction inputAction = InputAction.Interaction;

    [SerializeField]
    private UnityEvent onKeyDownEvent;

    private Action<InputAction, float> keyDownAction;
    #endregion

    private void Awake()
    {
        if(onKeyDownEvent.GetPersistentEventCount() > 0)
        {
            keyDownAction = (InputAction x, float y) => onKeyDownEvent.Invoke();
        }

        onEnter += OnEnter;
        onEnter += ListeningEvent;
        onExit += OnExit;
        onExit += StopListeningEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollide || isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            isEnter = true;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();

            tutorialController?.StartTutorial(tutorialType, onExit, tutorialName);
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoEnd || !isEnter) return;

        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            int len = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, layerMask).Length;
            if (len != 0)
            {
                Debug.Log("RETURN ~~~");
                return;
            }

            isEnter = false;
            tutorialController ??= other.gameObject.GetComponent<TutorialController>();
            tutorialController?.StopTutorial(tutorialType);
            onExit?.Invoke();
        }
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
            InputManager.StartListeningInput(inputAction, keyDownAction);
        }
    }

    private void StopListeningEvent()
    {
        if (keyDownAction != null)
        {
            InputManager.StopListeningInput(inputAction, keyDownAction);
        }
    }
}
