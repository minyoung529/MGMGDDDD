using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MoveState {
    [SerializeField] private PlayerStateName stateName;
    public override PlayerStateName StateName => stateName;

    public override void OnInput(Vector3 inputDir) {
        //do nothing
    }
}
