using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnswerData
{
    public TimeType time;
    public float minAngle;
    public float maxAngle;
}

public enum TimeType {
    Morning = 0,
    Afternoon = 1,
    Evening = 2,
    Night = 3,

    Count
}

