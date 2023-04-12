using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGroup : MonoBehaviour
{
    private PressurePlate[] plates;
    private bool isSelected = false;

    #region Property
    public int Value { get; private set; }
    public OperatorType OperatorType { get; private set; }
    private Action onSelectedAction;
    #endregion

    private void Awake()
    {
        plates = transform.GetComponentsInChildren<PressurePlate>();

        foreach (PressurePlate plate in plates)
        {
            plate.OnSelectedAction += OnSelected;
            plate.IsLock += IsLock;
        }
    }

    private void OnSelected(PressurePlate pressurePlate)
    {
        if (isSelected) return;
        isSelected = true;

        if (pressurePlate.GetType() == typeof(NumberPlate))  // Number Plate
        {
            Value = (pressurePlate as NumberPlate).Number;
        }
        else
        {
            OperatorType = (pressurePlate as OperatorPlate).OperatorType;
        }

        onSelectedAction?.Invoke();
    }

    public void ListeningOnSelected(Action action)
    {
        onSelectedAction -= action;
        onSelectedAction += action;
    }

    public void ResetPuzzle()
    {
        foreach (PressurePlate plate in plates)
        {
            plate.ResetPuzzle();
        }

        isSelected = false;
        Value = 0;
        OperatorType = OperatorType.None;
    }

    private bool IsLock() => isSelected;

    public void SetNumberPairs(Pair<int, int>[] pair)
    {
        for (int i = 0; i < plates.Length; i++)
        {
            if (plates[i] is NumberPlate)
            {
                (plates[i] as NumberPlate).SetNumberPair(pair[i]);
            }
        }
    }

    public void SetOperators(OperatorType[] operators)
    {
        for (int i = 0; i < plates.Length; i++)
        {
            if (plates[i] is OperatorPlate)
            {
                (plates[i] as OperatorPlate).SetOperator(operators[i]);
            }
        }
    }
}
