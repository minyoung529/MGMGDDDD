using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TogglePosition : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    [SerializeField] private Vector3 targetPos;
    private Vector3 originalPos;
    private bool isOpen = false;

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
        Debug.Log("¤Ó¤½;");
    }

    public void Open()
    {
        transform.DOKill();
        transform.DOMove(originalPos + targetPos, duration);
    }

    public void Close()
    {
        transform.DOKill();
        transform.DOMove(originalPos, duration);
    }
}
