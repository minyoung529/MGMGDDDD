using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThermometerController : MonoBehaviour
{
    private List<Thermometer> thermometers;

    [SerializeField]
    private float clearValue = 0.5f;

    [SerializeField]
    private UnityEvent onClearPuzzle;

    private bool isClear = false;

    private void Start()
    {
        thermometers = new List<Thermometer>(transform.GetComponentsInChildren<Thermometer>());

        foreach(Thermometer th in  thermometers)
        {
            th.OnChangeValue += CheckClear;
        }
    }

    private void CheckClear()
    {
        if (isClear) return;

        foreach (Thermometer thermometer in thermometers)
        {
            if (!thermometer.IsClear())
                return;
        }

        isClear = true;
        onClearPuzzle?.Invoke();
    }
}
