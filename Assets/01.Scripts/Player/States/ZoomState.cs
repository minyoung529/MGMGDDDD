using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomState : MoveState
{
    #region abstract ±¸ÇöºÎ
    public override StateName StateName => StateName.Zoom;

    public override void OnInput(Vector3 inputDir) {
        Player.SetRotate(Player.Forward);
        if (inputDir.sqrMagnitude <= 0) {
            Player.Decelerate();
        }
        else {
            Player.Accelerate(inputDir, maxSpeed: zoomSpeed);
            moveDir = inputDir;
        }
        Player.SetAnimInput(moveDir.normalized * (Player.CurSpeed / zoomSpeed));
    }
    #endregion

    [SerializeField] private float zoomSpeed = 2f;
    private Vector3 moveDir = Vector3.zero;

    private void Awake() {
        Player = GetComponent<PlayerMove>();
    }
}
