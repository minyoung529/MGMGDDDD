using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialPuzzleUI : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] Image[] slideImages;

    private float slideSpeed = 1f;
    public float MaxFillAmount { get; set; }

    private void Awake()
    {
        SetUIVisible(false);
    }

    public void SetUIVisible(bool value)
    {
        canvas.gameObject.SetActive(value);
    }

    public void SetHintText(string str)
    {
        hintText.SetText(str);
    }

    public void SetRadiusSlide(float value)
    {
        foreach(Image slide in slideImages)
        {
            slide.fillAmount = 1 - (value/ MaxFillAmount);
        }
    }

}
