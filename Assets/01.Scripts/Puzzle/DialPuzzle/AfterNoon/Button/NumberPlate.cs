using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NumberPlate : PressurePlate
{
    [SerializeField]
    private int number;

    #region PROPERTY
    public int Number => number;
    #endregion 

    public override void OnSelected()
    {

    }
}
