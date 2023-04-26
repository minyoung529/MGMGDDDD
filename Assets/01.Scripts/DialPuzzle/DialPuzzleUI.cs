using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialPuzzleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] Image[] slideImages;

    private float slideSpeed = 1f;
    public float MaxFillAmount { get; set; }

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
