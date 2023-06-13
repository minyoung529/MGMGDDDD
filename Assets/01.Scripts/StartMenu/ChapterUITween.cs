using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChapterUITween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image selectImage;

    [SerializeField]
    private Image outline;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = Color.white;
        color.a = 4 / 255f;
        selectImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectImage.color = Color.clear;
    }

    public void Select()
    {
        outline.color = Color.yellow;
    }

    public void UnSelect()
    {
        outline.color = Color.white;
    }
}
