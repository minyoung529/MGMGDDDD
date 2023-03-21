using System.Collections;
using UnityEngine;

public class JumpState : MoveState 
{
    #region abstarct 구현 부분
    public override StateName StateName => StateName.Jump;

    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            base.Player.Decelerate(brake);
            return;
        }
        base.Player.Accelerate(inputDir, accel, brake, maxSpeed);
        base.Player.SetRotate(inputDir);
    }
    #endregion

    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float brake = 5f;

    private int hash_tLanding = Animator.StringToHash("tLanding");

    public void Jump() {
        Player.Rigid.AddForce(Vector3.up * jumpPower, ForceMode.Force);
        StartCoroutine(LandingCoroutine());
    }

    private IEnumerator LandingCoroutine() {
        yield return new WaitForSeconds(1f);
        while(!base.Player.CheckOnGround()) {
            yield return null;
        }
        Player.Anim.SetTrigger(hash_tLanding);
    }
}
