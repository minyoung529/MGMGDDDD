using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TogglePosition : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private Vector3 targetPos;
    private Vector3 originalPos;
    private bool isOpen = false;
    public Vector3 MoveDir => targetPos;

    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnClose;
    [SerializeField] private UnityEvent OnEndOpen;
    [SerializeField] private UnityEvent OnEndClose;


    [SerializeField] Ease ease = Ease.Unset;

    [SerializeField] bool isPrevKill = true;

    [SerializeField]
    private bool isLocal = false;

    public float Duration => duration;

    private void Awake()
    {
        if (isLocal)
        {
            originalPos = transform.localPosition;
        }
        else
        {
            originalPos = transform.position;
        }
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        if (isOpen)
            Close();
        else
            Open();

        isOpen = !isOpen;
    }

    public void DelayToggle(float delay)
    {
        StartCoroutine(Delay(delay));
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Trigger();
    }

    [ContextMenu("Open")]
    public void Open()
    {
        if (isPrevKill)
        {
            transform.DOKill();
            StopAllCoroutines();
        }

        if (delay == 0f)
        {
            StartOpenSystem();
        }
        else
        {
            StartCoroutine(Delay(StartOpenSystem));
        }
    }

    private void StartOpenSystem()
    {
        if (isLocal)
        {
            transform.DOLocalMove(originalPos + targetPos, duration).SetEase(ease).OnComplete(OnEndOpen.Invoke);
        }
        else
        {
            transform.DOMove(originalPos + targetPos, duration).SetEase(ease).OnComplete(OnEndOpen.Invoke);
        }

        OnOpen?.Invoke();
    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (isPrevKill)
        {
            transform.DOKill();
            StopAllCoroutines();
        }

        if (delay == 0f)
        {
            StartCloseSystem();
        }
        else
        {
            StartCoroutine(Delay(StartCloseSystem));
        }
    }

    private void StartCloseSystem()
    {
        if (isLocal)
        {
            transform.DOLocalMove(originalPos, duration).SetEase(ease).OnComplete(OnEndClose.Invoke);
        }
        else
        {
            transform.DOMove(originalPos, duration).SetEase(ease).OnComplete(OnEndClose.Invoke);
        }

        OnClose?.Invoke();
    }

    public void ForceClosePosition()
    {
        if (isLocal)
            transform.localPosition = originalPos;
        else
            transform.position = originalPos;
    }

    public void ForceOpenPosition()
    {
        if (isLocal)
            transform.localPosition = originalPos + targetPos;
        else
            transform.position = originalPos + targetPos;
    }

    private IEnumerator Delay(System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}
