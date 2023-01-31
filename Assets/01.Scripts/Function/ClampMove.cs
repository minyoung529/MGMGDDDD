using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampMove : MonoBehaviour
{
    [Flags]
    public enum Axis
    {
        None = 0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
        All = int.MaxValue
    }

    [SerializeField] Axis axis;
    [SerializeField] Vector3 limitValue;
    [SerializeField] bool isLocal = false;

    Vector3 startPoint = Vector3.zero;

    void Start()
    {
        SetStartPoint();
    }

    void Update()
    {
        SetUpdateValue();
    }

    void SetStartPoint()
    {
        if (isLocal)
        {
            startPoint = transform.localPosition;
        }
        else
        {
            startPoint = transform.position;
        }
    }

    private void SetUpdateValue()
    {
        Vector3 curpos = (isLocal) ? transform.localPosition : transform.position;
        curpos -= startPoint;

        if ((curpos - startPoint).sqrMagnitude < 0.01f) return;

        if ((axis & Axis.X) != 0)
        {
            curpos.x = GetClampValue(curpos.x, limitValue.x);
        }
        
        if ((axis & Axis.Y) != 0)
        {
            curpos.y = GetClampValue(curpos.y, limitValue.y);
        }
        
        if ((axis & Axis.Z) != 0)
        {
            curpos.z = GetClampValue(curpos.z, limitValue.z);
        }

        if (isLocal)
        {
            transform.localPosition = startPoint + curpos;
        }
        else
        {
            transform.position = startPoint + curpos;
        }
    }

    float GetClampValue(float curAxis, float value)
    {
        return Math.Clamp(curAxis, Math.Min(0f, value), Math.Max(0f, value)); ;
    }
}
