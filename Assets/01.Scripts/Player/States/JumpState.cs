using System.Collections;
using UnityEngine;

public class JumpState : MoveState 
{
    #region abstarct 구현 부분
    public override StateName StateName => StateName.Jump;
    private PlayerMove player = null;
    public override PlayerMove PlayerMove => player;

    public override void OnStateStart() {

    }
    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            PlayerMove.Decelerate(brakeTime);
            return;
        }
        player.Accelerate(inputDir, accel, brakeTime, maxSpeed);
        player.SetRotate(inputDir);
    }
    #endregion

    private Rigidbody rigid = null;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float accel = 1f;
    [SerializeField] private float brakeTime = 3f;


    private Animator anim = null;
    private int hash_tLanding = Animator.StringToHash("tLanding");

    private bool isJump = false;

    private void Awake() {
        player = GetComponent<PlayerMove>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    public void Jump() {
        if (isJump) return;
        isJump = true;
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Force);
        StartCoroutine(LandingCoroutine());
    }

    private IEnumerator LandingCoroutine() {
        yield return new WaitForSeconds(1f);
        while(!player.CheckOnGround()) {
            yield return null;
        }
        anim.SetTrigger(hash_tLanding);
        isJump = false;
    }
}
