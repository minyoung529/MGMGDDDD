using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Thermometer : MonoBehaviour
{
    private readonly float MAX_LIQUID_SCALE_Y = 3f;
    private readonly float LIQUID_MOVE_SPEED = 4f;

    private Sequence seq;

    private float NormalizedLiquid => Mathf.Lerp(0f, 1f, liquidPivot.localScale.y / MAX_LIQUID_SCALE_Y);

    // Set Normalize Value
    [SerializeField]
    private Transform liquidPivot;

    [SerializeField, Range(0f, 1f)]
    private float beginLiquidValue;

    private void Start()
    {
        SetLiquidValueBy01(beginLiquidValue);
    }

    public void AddNormalizedLiquidValue(float value)
    {
        Debug.Log(NormalizedLiquid + value);
        ChangeNormalizedLiquidValue(NormalizedLiquid + value);
    }

    public void ChangeNormalizedLiquidValue(float normalizedValue)
    {
        normalizedValue = Mathf.Clamp01(normalizedValue);
        float scaleY = Mathf.Lerp(0f, MAX_LIQUID_SCALE_Y, normalizedValue);
        float distance = Mathf.Abs(scaleY - liquidPivot.localScale.y);

        seq?.Kill();
        seq = DOTween.Sequence();

        seq.Append
        (
            DOTween.To(() => liquidPivot.localScale.y,
            (x) => SetLiquidValueByScale(x),
            scaleY,
            LIQUID_MOVE_SPEED * distance).SetEase(Ease.Linear)
        );
    }

    private void SetLiquidValueByScale(float scaleY)
    {
        Vector3 scale = liquidPivot.localScale;
        scale.y = scaleY;

        liquidPivot.localScale = scale;
    }

    private void SetLiquidValueBy01(float normalized)
    {
        Vector3 scale = liquidPivot.localScale;
        scale.y = Mathf.Lerp(0f, MAX_LIQUID_SCALE_Y, normalized);

        liquidPivot.localScale = scale;
    }

    private void OnValidate()
    {
        SetLiquidValueBy01(beginLiquidValue);
    }
}
