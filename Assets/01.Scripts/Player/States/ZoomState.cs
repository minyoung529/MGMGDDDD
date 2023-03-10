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
        if (inputDir.sqrMagnitude <= 0)
            player.Decelerate();
        else
            player.Accelerate(inputDir);
    }
    #endregion

    private void Awake() {
        player = GetComponent<PlayerMove>();
    }

    private void SetAnim() {
        player.Anim.SetFloat(hash_fVertical, Vector3.Dot(Forward, inputDir));
        player.Anim.SetFloat(hash_fHorizontal, Vector3.Dot(Right, inputDir));
    }
}
