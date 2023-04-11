using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerChanger : MonoBehaviour
{
    private bool controlLiquid = false;
    public bool ControlLiquid
    {
        get => controlLiquid;
        set
        {
            controlLiquid = value;
        }
    }

    private float maxValue = 6f;
    private float minValue;

    [SerializeField]
    private Transform pivot;

    public void Initialize(float maxValue)
    {
        this.maxValue = maxValue;
        pivot = transform.parent;
    }

    public void SetMinimum(float min)
    {
        minValue = min * maxValue;
    }

    private void LateUpdate()
    {
        // SET LIMIT
        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        localPos.y = Mathf.Clamp(localPos.y, minValue, maxValue);
        transform.position = pivot.TransformPoint(localPos);
    }

    public void SetLiquidValue(float value)
    {
        Vector3 localPos = transform.localPosition;
        localPos.y = Mathf.Clamp(value * maxValue, minValue, maxValue);
        transform.position = pivot.TransformPoint(localPos);
    }

    public float GetNormalizeValue()
    {
        Vector3 localPos = pivot.InverseTransformPoint(transform.position);
        return localPos.y / maxValue;
    }

    #region DEBUG
    [ContextMenu("ControlLiquid_True")]
    public void SetControlLiquidTrue() => ControlLiquid = true;

    [ContextMenu("ControlLiquid_False")]
    public void SetControlLiquidFalse() => ControlLiquid = false;
    #endregion
}
