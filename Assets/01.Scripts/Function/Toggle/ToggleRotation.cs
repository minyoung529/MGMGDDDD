using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToggleRotation : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetAngles;
    private Vector3 originalAngles;
    private bool isOpen = false;

    private void Start()
    {
        originalAngles = transform.eulerAngles;
    }

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
        transform.DORotate(targetAngles, duration);
    }

    public void Close()
    {
        transform.DOKill();
        transform.DORotate(originalAngles, duration);
    }
}
