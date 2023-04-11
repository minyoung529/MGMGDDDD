using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGroup : MonoBehaviour
{
    private PressurePlate[] plates;
    private bool isSelected = false;

    public int Value { get; private set; }
    public OperatorType OperatorType { get; private set; }

    private void Awake()
    {
        plates = transform.GetComponentsInChildren<PressurePlate>();

        foreach(PressurePlate plate in plates)
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
    }

    private bool IsLock() => isSelected;

    public void ResetPuzzle()
    {
        foreach (PressurePlate plate in plates)
        {
            plate.ResetPuzzle();
        }

        isSelected = false;
    }
}
