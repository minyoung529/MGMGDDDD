using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NumberPlate : PressurePlate
{
    public int Number { get; private set; }
    private Pair<int, int> numberPair;

    public override void OnSelected()
    {

    }

    public void OnFire()
    {
        Number = numberPair.second;
    }

    public void SetNumberPair(Pair<int, int> numberPair)
    {
        this.numberPair = numberPair;
        Number = numberPair.first;

        if (numberPair.second < 0)
        {
            numberPair.second = numberPair.first;
        }
    }
}
