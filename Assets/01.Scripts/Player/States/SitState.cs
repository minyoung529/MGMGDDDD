using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitState : MoveState {
    public override StateName StateName => StateName.Sit;

    public override void OnInput(Vector3 inputDir) {
        //do nothing
    }
}
