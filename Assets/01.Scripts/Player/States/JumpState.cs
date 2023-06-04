using System;
using System.Collections;
using UnityEngine;

public class JumpState : MoveState 
{
    #region abstarct 구현 부분
    public override PlayerStateName StateName => PlayerStateName.Jump;

    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            Player.Decelerate(brake);
            return;
        }
        Player.Accelerate(inputDir, accel, brake, maxSpeed);
        Player.SetRotate(inputDir);
    }

    public override void OnStateEnd(Action onChange) {
        StopAllCoroutines();
        Player.Controller.Anim.SetBool(hash_bLanding, false);
        base.OnStateEnd(onChange);
    }
    #endregion

    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float brake = 5f;

    private int hash_bLanding = Animator.StringToHash("bLanding");

    public void Jump() {
        Vector3 dir = Player.Controller.Rigid.velocity;
        dir.y = 0;
        Player.Controller.Rigid.velocity = dir;
        Player.Controller.Rigid.AddForce(Vector3.up * jumpPower, ForceMode.Force);
        StartCoroutine(LandingCoroutine());
    }

    private IEnumerator LandingCoroutine() {
        yield return new WaitForSeconds(0.2f);
        while(!Player.CheckOnGround()) {
            yield return null;
        }
        Player.Controller.Anim.SetBool(hash_bLanding, true);
    }
}
