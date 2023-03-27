using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions : MonoBehaviour
{
    [SerializeField]
    private List<Condition> conditions;

    public bool Condition()
    {
        foreach(Condition condition in conditions)
        {
            if (!condition.ConditionFunc())
                return false;
        }

        return true;
    }
}
