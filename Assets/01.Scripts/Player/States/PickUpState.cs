using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpState : MoveState {
    public override StateName StateName => StateName.PickUp;

    public override void OnInput(Vector3 inputDir) {
        //do nothing
    }
}
