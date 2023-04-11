using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MathPuzzleController : MonoBehaviour
{
    [Header("GROUP")]
    [SerializeField]
    private PlateGroup firstGroup;

    [SerializeField]
    private PlateGroup operators;

    [SerializeField]
    private PlateGroup secondGroup;

    [field: SerializeField]
    public int TargetValue { get; set; }

    [Header("CLEAR")]
    [SerializeField]
    private UnityEvent onClearPuzzle;

    #region PROPERTY
    public int FirstNumber => firstGroup.Value;
    public OperatorType Operator => operators.OperatorType;
    public int SecondNumber => secondGroup.Value;
    #endregion

    private void Update()
    {
        Debug.Log(CalculatedValue());
    }

    private void ResetPuzzle()
    {
        firstGroup.ResetPuzzle();
        operators.ResetPuzzle();
        secondGroup.ResetPuzzle();
    }

    private int CalculatedValue() => Operator switch
    {
        OperatorType.Plus => FirstNumber + SecondNumber,
        OperatorType.Minus => FirstNumber - SecondNumber,
        OperatorType.Multiply => FirstNumber * SecondNumber,
        OperatorType.Divide => FirstNumber * SecondNumber,
        _ => 0
    };

    public void CalculateResult()
    {
        if (FirstNumber == 0 || Operator == OperatorType.None || SecondNumber == 0)
        {
            return;
        }

        if (CalculatedValue() == TargetValue)
        {
            onClearPuzzle?.Invoke();
            Debug.Log("CLEAR");
        }
    }
}
