using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectedUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    [SerializeField]
    private float xMovement = 100f;
    private float originalPosX;

    [SerializeField]
    private float duration;

    [SerializeField]
    private UnityEvent onEnter;

    void Awake()
    {
        rectTransform = transform as RectTransform;
        originalPosX = rectTransform.anchoredPosition.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(originalPosX + xMovement, duration);
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(originalPosX, duration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerExit(null);
    }
}
