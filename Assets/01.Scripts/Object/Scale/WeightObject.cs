using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WeightObject : MonoBehaviour
{
    [SerializeField] private float mass = 0f;
    private bool inWeight = false;

    public float GetMass{ get { return mass; }}
    public bool InWeight { get { return inWeight; } set { inWeight = value; } }

    private BalanceFloor floor;

    public void DecreaseMass(float value)
    {
        mass -= value;
        if (mass <= 1f)
        {
            mass = 1f;
        }
    }
    public void IncreaseMass(float value)
    {
        mass += value;
    }

    public void EnterFloor(BalanceFloor _floor)
    {
        if(InWeight)
        {
            if (floor != _floor) floor.DecreaseWeight(mass);
            else return;
        }

        InWeight = true;
        floor = _floor;
        floor.IncreaseWeight(mass);
    }
    public void ExitFloor(BalanceFloor _floor)
    {
        floor = null;
        InWeight = false;
    }
}
