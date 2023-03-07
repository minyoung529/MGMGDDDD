using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToggleRotation : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetAngles;
    [SerializeField] private bool isLocal = false;
    private Vector3 originalAngles;
    private bool isOpen = false;

    private void Start()
    {
        if (isLocal)
        {
            originalAngles = transform.localEulerAngles;
        }
        else
        {
            originalAngles = transform.eulerAngles;
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
            transform.DOLocalRotate(targetAngles, duration);
        else
            transform.DORotate(targetAngles, duration);
    }

    public void Close()
    {
        transform.DOKill();

        if (isLocal)
            transform.DOLocalRotate(originalAngles, duration);
        else
            transform.DORotate(originalAngles, duration);
    }
}
