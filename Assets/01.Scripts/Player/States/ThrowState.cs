using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : MoveState
{
    public override StateName StateName => StateName.Throw;

    public override void OnInput(Vector3 inputDir) {
        //do nothing
    }
}
