using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        onEnter += OnEnter;
        onExit += OnExit;
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

        if (((1 << other.gameObject.layer) & layerMask) != 0 && Condition(other.transform))
        {
            int len = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, transform.rotation, layerMask).Length;
            if (len != 0) return;

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
}
