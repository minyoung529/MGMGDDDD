using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnswerData {
    public TimeType time;
    public float minAngle;
    public float maxAngle;
}

[System.Serializable]
public struct AnswerList {
    public TimeType this[int i] {
        get {
            return array[i];
        }
        set {
            if (i >= array.Count) {
                return;
            }
            array[i] = value;
        }
    }
    public List<TimeType> array;
}

public enum TimeType {
    None = -1,

    Morning = 0,
    MorningToAfternoon = 1,
    
    Afternoon = 2,
    AfternoonToEvening = 3,
    
    Evening = 4,
    EveningToNight = 5,

    Night = 6,
    NightToMorning = 7,

    Count
}