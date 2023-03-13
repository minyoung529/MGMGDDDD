using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomState : MoveState
{
    #region abstract ±¸ÇöºÎ
    public override StateName StateName => StateName.Zoom;
    private PlayerMove player = null;
    public override PlayerMove PlayerMove => player;

    public override void OnInput(Vector3 inputDir) {
        player.SetRotate(player.Forward);
        if (inputDir.sqrMagnitude <= 0) {
            player.Decelerate();
        }
        else {
            player.Accelerate(inputDir, maxSpeed: zoomSpeed);
            moveDir = inputDir;
        }
        player.SetAnimInput(moveDir.normalized * (player.CurSpeed / zoomSpeed));
    }
    #endregion

    [SerializeField] private float zoomSpeed = 2f;
    private Vector3 moveDir = Vector3.zero;

    private void Awake() {
        player = GetComponent<PlayerMove>();
    }
}
