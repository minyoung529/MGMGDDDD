using UnityEngine;

public class DefaultMoveState : MoveState 
{
    #region abstarct 구현 부분
    public override StateName StateName => StateName.DefaultMove;

    private PlayerMove player = null;
    public override PlayerMove PlayerMove => player;

    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            Stop();
            return;
        }
        anim.SetBool(hash_bWalk, true);
        player.Accelerate(inputDir, accel, brakeTime, MaxSpeed);
        player.SetRotate(inputDir, rotateTime);
    }
    #endregion

    #region 속도, 방향
    private Rigidbody rigid = null;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float brakeTime = 0.5f;
    [SerializeField] private float accel = 1f;
    public float MaxSpeed { get; private set; }
    #endregion

    #region 애니메이션
    private Animator anim = null;
    private int hash_bWalk = Animator.StringToHash("bWalk");
    private int hash_bSprint = Animator.StringToHash("bSprint");
    private int hash_tStop = Animator.StringToHash("tStop");
    #endregion

    private void Awake() {
        player = GetComponent<PlayerMove>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        MaxSpeed = walkSpeed;

        InputManager.StartListeningInput(InputAction.Sprint, Sprint);
    }

    private void Sprint(InputAction action, float param) {
        if (!anim.GetBool(hash_bWalk)) return;
        anim.SetBool(hash_bSprint, true);
        MaxSpeed = sprintSpeed;
    }

    private void Stop() {
        if (anim.GetBool(hash_bSprint) && player.CurSpeed > (walkSpeed + sprintSpeed) / 2) {
            anim.SetTrigger(hash_tStop);
        }
        MaxSpeed = walkSpeed;
        anim.SetBool(hash_bSprint, false);
        anim.SetBool(hash_bWalk, false);
        player.Decelerate(brakeTime);
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }
}
