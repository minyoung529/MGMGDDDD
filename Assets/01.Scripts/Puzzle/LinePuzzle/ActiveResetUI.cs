using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class ActiveResetUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas;

    [SerializeField]
    private UnityEvent onActive;

    [SerializeField]
    private UnityEvent onInactive;

    public void Active()
    {
        gameObject.SetActive(true);
        canvas.DOFade(1f, 1f);

        onActive?.Invoke();
    }

    public void Inactive()
    {
        onInactive?.Invoke();
        gameObject.SetActive(false);
    }
}
