using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TogglePosition : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetPos;
    private Vector3 originalPos;
    private bool isOpen = false;
    public Vector3 MoveDir => targetPos;

    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnOpenComplete;
    [SerializeField] private UnityEvent OnClose;
    [SerializeField] private UnityEvent OnCloseComplete;

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
            transform.DOKill();

        if (isLocal)
        {
            transform.DOLocalMove(originalPos + targetPos, duration).OnComplete(() => OnOpenComplete?.Invoke()).SetEase(ease);
        }
        else
        {
            transform.DOMove(originalPos + targetPos, duration).OnComplete(() => OnOpenComplete?.Invoke()).SetEase(ease);
        }

        OnOpen?.Invoke();
    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (isPrevKill)
            transform.DOKill();

        if (isLocal)
        {
            transform.DOLocalMove(originalPos, duration).OnComplete(() => OnCloseComplete?.Invoke()).SetEase(ease);
        }
        else
        {
            transform.DOMove(originalPos, duration).OnComplete(() => OnCloseComplete?.Invoke()).SetEase(ease);
        }

        OnClose?.Invoke();
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}