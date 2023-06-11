using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ToggleScale : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetScale;
    private Vector3 originalScale;
    private bool isOpen = false;

    [SerializeField] private UnityEvent OnSize;
    [SerializeField] private UnityEvent OnSizeComplete;
    [SerializeField] private UnityEvent OnReSize;
    [SerializeField] private UnityEvent OnReSizeComplete;

    [SerializeField] Ease ease = Ease.Unset;

    [SerializeField] bool isPrevKill = true;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        if (isOpen)
            ReSize();
        else
            Size();

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

    [ContextMenu("Size")]
    public void Size()
    {
        if (isPrevKill)
            transform.DOKill();
        transform.DOScale(targetScale, duration).OnComplete(() => OnSizeComplete.Invoke()).SetEase(ease);
        OnSize?.Invoke();
    }

    [ContextMenu("ReSize")]
    public void ReSize()
    {
        if (isPrevKill)
            transform.DOKill();
        transform.DOScale(originalScale, duration).OnComplete(() => OnReSizeComplete.Invoke()).SetEase(ease);
        OnReSize?.Invoke();
    }

    public void ForceSize()
    {
        transform.DOKill();
        transform.localScale = targetScale;
    }

    public void ForceResize()
    {
        transform.DOKill();
        transform.localScale = originalScale;
    }

    public void SetTargetScale(Vector3 targetScale) {
        this.targetScale = targetScale;
    }
}
