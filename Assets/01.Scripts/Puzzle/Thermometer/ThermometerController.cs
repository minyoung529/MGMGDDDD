using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerController : MonoBehaviour
{
    private List<Thermometer> thermometers;

    [SerializeField]
    private float clearValue = 0.5f;

    private void Start()
    {
        thermometers = new List<Thermometer>(transform.GetComponentsInChildren<Thermometer>());
    }

    private bool CheckClear()
    {
        foreach (Thermometer thermometer in thermometers)
        {
            if (!thermometer.IsClear(clearValue))
                return false;
        }

        return true;
    }
}
