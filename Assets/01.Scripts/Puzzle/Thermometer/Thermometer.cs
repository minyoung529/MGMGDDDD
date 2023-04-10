using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Thermometer : MonoBehaviour
{
    private readonly float MAX_LIQUID_SCALE_Y = 3f;
    private readonly float LIQUID_MOVE_SPEED = 4f;

    private Sequence seq;

    private float NormalizedLiquid => NormalizedRed + NormalizedYellow;
    private float NormalizedRed => Mathf.Clamp01(Mathf.Lerp(0f, 1f, redLiquidPivot.localScale.y / MAX_LIQUID_SCALE_Y));
    private float NormalizedYellow => Mathf.Clamp01(Mathf.Lerp(0f, 1f, yellowLiquidPivot.localScale.y / MAX_LIQUID_SCALE_Y));

    private float prevRedValue = 0f;
    private float RedScaleY
    {
        get => redLiquidPivot.localScale.y;
        set => SetLiquidValueByScale(value, redLiquidPivot);
    }
    private float YellowScaleY
    {
        get => yellowLiquidPivot.localScale.y;
        set => SetLiquidValueByScale(value, yellowLiquidPivot);
    }

    // Set Normalize Value
    [SerializeField]
    private Transform redLiquidPivot;
    [SerializeField]
    private Transform yellowLiquidPivot;

    [SerializeField, Range(0f, 1f)]
    private float redBeginLiquidValue;

    [SerializeField, Range(0f, 1f)]
    private float yellowBeginLiquidValue;

    [SerializeField]
    private ThermometerChanger changer;

    private void Start()
    {
        SetLiquidValueBy01_R(redBeginLiquidValue);
        SetLiquidValueBy01_Y(yellowBeginLiquidValue);
        UpdateYellow();

        changer.Initialize(MAX_LIQUID_SCALE_Y * 2f);
    }

    private void Update()
    {
        if (changer.ControlLiquid)
        {
            RedScaleY = MAX_LIQUID_SCALE_Y;
            SetLiquidValueBy01_R(changer.GetNormalizeValue());
        }
        else
        {
            changer.SetLiquidValue(NormalizedLiquid);
        }
    }

    #region CHANGE LIQUID VALUE
    public void AddNormalizedLiquidValue(float value)
    {
        ChangeTween(redLiquidPivot, Mathf.Clamp01(NormalizedRed + value));
        prevRedValue = NormalizedRed;
    }

    public void AddNormalizedLiquidValue_Y(float value)
    {
        ChangeTween(yellowLiquidPivot, Mathf.Clamp(NormalizedYellow + value, 0f, 1f - NormalizedRed));
    }

    private void ChangeTween(Transform trn, float normalized)
    {
        float scaleY = Mathf.Lerp(0f, MAX_LIQUID_SCALE_Y, normalized);
        float distance = Mathf.Abs(scaleY - trn.localScale.y);

        seq?.Kill();
        seq = DOTween.Sequence();

        seq.Append
        (
            DOTween.To(() => trn.localScale.y,
            (x) => SetLiquidValueByScale(x, trn),
            scaleY,
            LIQUID_MOVE_SPEED * distance).SetEase(Ease.Linear)
        );

        seq.onUpdate += UpdateYellow;
    }
    #endregion

    #region DIRECT SET LIQUID
    private void SetLiquidValueByScale(float scaleY, Transform trn)
    {
        if (trn == redLiquidPivot)
        {
            prevRedValue = NormalizedRed;
        }

        Vector3 scale = trn.localScale;
        scale.y = scaleY;

        trn.localScale = scale;
    }

    private void SetLiquidValueBy01_R(float normalized)
    {
        prevRedValue = NormalizedRed;
        RedScaleY = NormalizedToScaleY(normalized);
    }

    private void SetLiquidValueBy01_Y(float normalized)
    {
        Vector3 yellowPosition = yellowLiquidPivot.localPosition;
        yellowPosition.y = RedScaleY * 2f - 0.1f;
        yellowLiquidPivot.localPosition = yellowPosition;

        normalized = Mathf.Clamp(normalized, 0f, 1f - NormalizedRed);
        YellowScaleY = NormalizedToScaleY(normalized);
    }

    private void UpdateYellow()
    {
        float diff = Mathf.Abs(prevRedValue - NormalizedRed);
        SetLiquidValueBy01_Y(NormalizedYellow - diff);

        prevRedValue = NormalizedRed;
    }

    private float NormalizedToScaleY(float value)
    {
        return Mathf.Lerp(0f, MAX_LIQUID_SCALE_Y, value);
    }
    #endregion

    public bool IsClear(float weight)
    {
        return (Mathf.Abs(NormalizedLiquid - weight) < 0.1f);
    }

    private void OnValidate()
    {
        SetLiquidValueBy01_R(redBeginLiquidValue);
        SetLiquidValueBy01_Y(yellowBeginLiquidValue);
        UpdateYellow();
    }
}
