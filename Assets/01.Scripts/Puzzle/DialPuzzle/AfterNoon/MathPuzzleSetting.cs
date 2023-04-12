using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MathPuzzleSetting
{
    public int targetValue;
    
    public Pair<int, int>[] firstNumbers;
    public OperatorType[] operatorTypes;
    public Pair<int, int>[] secondNumbers;
}
