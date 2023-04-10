using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxisController
{
    [SerializeField]
    private AxisControlType axisControlType;
    private Vector3 localAxis = Vector3.one;
    private Transform transform;

    #region BASE
    public AxisController(Transform transform)
    {
        this.transform = transform;
    }
    #endregion

    #region FLAG CONTROL
    public void AddAxis(AxisControlType controlType)
    {
        axisControlType |= controlType;
    }

    public void RemoveAxis(AxisControlType controlType)
    {
        axisControlType &= ~controlType;
    }

    public void SetAxis(AxisControlType controlType)
    {
        axisControlType = controlType;
    }
    #endregion

    #region DISTANCE
    public void SetLocaAxis(Vector3 localAxis)
    {
        this.localAxis = localAxis;
    }

    public Vector3 CalculateDestination(Vector3 dest)
    {
        if (axisControlType == AxisControlType.None)
            return dest;

        Vector3 curPos = transform.position;

        if (axisControlType == AxisControlType.PlusX || axisControlType == AxisControlType.MinusX)
        {
            dest.z = curPos.z;

            if ((axisControlType == AxisControlType.PlusX && curPos.x > dest.x) ||
                (axisControlType == AxisControlType.MinusX && curPos.x < dest.x))
            {
                dest.x = curPos.x;
            }
        }
        else if (axisControlType == AxisControlType.PlusZ || axisControlType == AxisControlType.MinusZ)
        {
            dest.x = curPos.x;

            if ((axisControlType == AxisControlType.PlusZ && curPos.z > dest.z) ||
                (axisControlType == AxisControlType.MinusZ && curPos.z < dest.z))
            {
                dest.z = curPos.z;
            }
        }

        return dest;
    }


    #endregion
}
