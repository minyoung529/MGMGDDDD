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

    [SerializeField] private UnityEvent OnOpen;

    private void Start()
    {
        originalPos = transform.position;
    }

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

    public void Open()
    {
        transform.DOKill();
        transform.DOMove(originalPos + targetPos, duration);

        OnOpen?.Invoke();
    }

    public void Close()
    {
        transform.DOKill();
        transform.DOMove(originalPos, duration);
    }
}
