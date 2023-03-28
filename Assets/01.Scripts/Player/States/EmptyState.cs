using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MoveState {
    [SerializeField] private StateName stateName;
    public override StateName StateName => stateName;

    public override void OnInput(Vector3 inputDir) {
        //do nothing
    }
}
