using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ToggleRotation : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetAngles;
    [SerializeField] private bool isLocal = false;
    private Quaternion originalAngles;
    private bool isOpen = false;

    [SerializeField]
    private float predelay = 0f;

    private void Start()
    {
        if (isLocal)
        {
            originalAngles = transform.localRotation;
        }
        else
        {
            originalAngles = transform.rotation;
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

    public void Open()
    {
        transform.DOKill();

        if (isLocal)
        {
            Delay(() => transform.DOLocalRotateQuaternion(Quaternion.Euler(targetAngles), duration));
        }
        else
        {
            Delay(() => transform.DORotateQuaternion(Quaternion.Euler(targetAngles), duration));
        }
    }

    public void Close()
    {
        Debug.Log("Close");
        transform.DOKill();

        if (isLocal)
        {
            Delay(() => transform.DOLocalRotateQuaternion(originalAngles, duration));
        }
        else
        {
            Delay(() => transform.DORotateQuaternion(originalAngles, duration));
        }
    }

    private void Delay(Action action)
    {
        if (predelay > 0f)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(predelay);
            seq.AppendCallback(() => action?.Invoke());
        }
        else
        {
            action?.Invoke();
        }
    }
}
