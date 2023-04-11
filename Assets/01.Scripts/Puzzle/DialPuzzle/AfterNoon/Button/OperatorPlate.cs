using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperatorType
{
    None,
    Plus,
    Minus, 
    Multiply,
    Divide,

    Count
}

public sealed class OperatorPlate : PressurePlate
{
    [SerializeField]
    private OperatorType operatorType;

    #region Property
    public OperatorType OperatorType => operatorType;
    #endregion

    public override void OnSelected()
    {

    }
} 