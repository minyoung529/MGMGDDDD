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
    [SerializeField] private UnityEvent OnClose;

    [SerializeField] Ease ease = Ease.Unset;

    [SerializeField] bool isPrevKill = true;

    [SerializeField]
    private bool isLocal = false;

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
            transform.DOLocalMove(originalPos + targetPos, duration).SetEase(ease);
        }
        else
        {
            transform.DOMove(originalPos + targetPos, duration).SetEase(ease);
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
            transform.DOLocalMove(originalPos, duration).SetEase(ease);
        }
        else
        {
            transform.DOMove(originalPos, duration).SetEase(ease);
        }

        OnClose?.Invoke();
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}
