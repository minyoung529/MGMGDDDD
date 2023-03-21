using UnityEngine;

public class DefaultMoveState : MoveState 
{
    #region abstarct 구현 부분
    public override StateName StateName => StateName.DefaultMove;

    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            Stop();
            return;
        }
        Player.Anim.SetBool(hash_bWalk, true);
        Player.Accelerate(inputDir, accel, brake, MaxSpeed);
        Player.SetRotate(inputDir, rotateTime);
    }
    #endregion

    #region 속도, 방향
    private Rigidbody rigid = null;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float brake = 5f;
    [SerializeField] private float accel = 1f;
    public float MaxSpeed { get; private set; }
    #endregion

    #region 애니메이션
    private int hash_bWalk = Animator.StringToHash("bWalk");
    private int hash_bSprint = Animator.StringToHash("bSprint");
    private int hash_tStop = Animator.StringToHash("tStop");
    #endregion

    private void Awake() {
        Player = GetComponent<PlayerMove>();

        MaxSpeed = walkSpeed;

        InputManager.StartListeningInput(InputAction.Sprint, Sprint);
    }

    private void Sprint(InputAction action, float param) {
        if (!Player.Anim.GetBool(hash_bWalk)) return;
        Player.Anim.SetBool(hash_bSprint, true);
        MaxSpeed = sprintSpeed;
    }

    private void Stop() {
        if (Player.Anim.GetBool(hash_bSprint) && Player.CurSpeed > (walkSpeed + sprintSpeed) / 2) {
            Player.Anim.SetTrigger(hash_tStop);
            Player.LockInput(0.3f);
        }
        MaxSpeed = walkSpeed;
        Player.Anim.SetBool(hash_bSprint, false);
        Player.Anim.SetBool(hash_bWalk, false);
        Player.Decelerate(brake);
    }

    private void OnDestroy() {
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }
}
