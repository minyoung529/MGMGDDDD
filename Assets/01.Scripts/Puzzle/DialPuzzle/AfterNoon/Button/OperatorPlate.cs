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

    [SerializeField]
    private Transform[] operations;

    #region Property
    public OperatorType OperatorType => operatorType;
    #endregion

    public override void OnSelected()
    {

    }

    public void SetOperator(OperatorType operatorType)
    {
        this.operatorType = operatorType;

        ActiveOperations();
    }

    private void ActiveOperations()
    {
        foreach (Transform operation in operations)
            operation.gameObject.SetActive(false);

        operations[(int)operatorType - 1].gameObject.SetActive(true);
    }
} 