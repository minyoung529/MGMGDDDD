using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilContactCondition : Condition
{
    private HardMoveObject hardMoveObject;

    private void Awake()
    {
        hardMoveObject = GetComponent<HardMoveObject>();
    }

    public override bool ConditionFunc()
    {
        return hardMoveObject.CanMove;
    }
}
