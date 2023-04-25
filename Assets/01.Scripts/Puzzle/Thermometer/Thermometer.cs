using DG.Tweening;
using System;
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

    [SerializeField]
    private float targetValue;

    public Action OnChangeValue { get; set; }

    private void Start()
    {
        SetLiquidValueBy01_R(redBeginLiquidValue);
        SetLiquidValueBy01_Y(yellowBeginLiquidValue);
        UpdateYellow();
        UpdateRedPosition();

        changer.Initialize(MAX_LIQUID_SCALE_Y * 2f);
    }

    private void Update()
    {
        changer.SetMinimum(NormalizedYellow);

        if (changer.ControlLiquid)
        {
            SetLiquidValueBy01_R(changer.GetNormalizeValue() - NormalizedYellow);
            OnChangeValue?.Invoke();
        }
        else
        {
            changer.SetLiquidValue(NormalizedLiquid);
        }
    }

    #region CHANGE LIQUID VALUE
    public void AddNormalizedLiquidValue(float value)
    {
        ChangeTween(redLiquidPivot, Mathf.Clamp(NormalizedRed + value, 0f, 1f));
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
        ).OnComplete(() => OnChangeValue?.Invoke());

        seq.onUpdate += UpdateYellow;
        seq.onUpdate += UpdateRedPosition;
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
        normalized = Mathf.Clamp(normalized, 0f, 1f - NormalizedRed);
        YellowScaleY = NormalizedToScaleY(normalized);
    }
    #endregion

    #region UPDATE
    private void UpdateRedPosition()
    {
        Vector3 redPosition = redLiquidPivot.localPosition;
        redPosition.y = YellowScaleY * 2f - 0.1f;
        redLiquidPivot.localPosition = redPosition;
    }

    private void UpdateYellow()
    {
        float diff = Mathf.Abs(prevRedValue - NormalizedRed);
        SetLiquidValueBy01_Y(NormalizedYellow - diff);

        prevRedValue = NormalizedRed;
    }
    #endregion

    #region GET
    private float NormalizedToScaleY(float value)
    {
        return Mathf.Lerp(0f, MAX_LIQUID_SCALE_Y, value);
    }

    public bool IsClear()
    {
        return (Mathf.Abs(NormalizedLiquid - targetValue) < 0.05f);
    }
    #endregion

    #region TEST
    [ContextMenu("RED+0.1")]
    public void RedPlus01() => AddNormalizedLiquidValue(0.1f);

    [ContextMenu("YELLOW+0.1")]
    public void YellpwPlus01() => AddNormalizedLiquidValue_Y(0.1f);
    #endregion

    private void OnValidate()
    {
        SetLiquidValueBy01_R(redBeginLiquidValue);
        SetLiquidValueBy01_Y(yellowBeginLiquidValue);

        UpdateYellow();
        UpdateRedPosition();

        changer.SetLiquidValue(NormalizedLiquid);
    }
}
