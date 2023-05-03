using UnityEngine;

public class DefaultMoveState : MoveState
{
    #region 속도, 방향
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float brake = 5f;
    [SerializeField] private float accel = 1f;

    [SerializeField] private float oilWalkWeight = 1.2f;
    [SerializeField] private float oilSprintWeight = 1.2f;

    public float MaxSpeed { get; private set; }

    private float originSprintSpeed;
    private float originWalkSpeed;
    private float originBrake;
    #endregion

    #region 애니메이션
    private int hash_bWalk = Animator.StringToHash("bWalk");
    private int hash_bSprint = Animator.StringToHash("bSprint");
    private int hash_tStop = Animator.StringToHash("tStop");
    #endregion

    #region abstarct 구현 부분
    [SerializeField] private PlayerStateName stateName = PlayerStateName.DefaultMove;
    public override PlayerStateName StateName => stateName;

    public override void OnInput(Vector3 inputDir) {
        if (inputDir.sqrMagnitude <= 0) {
            Stop();
            return;
        }
        Player.Controller.Anim.SetBool(hash_bWalk, true);
        Player.Accelerate(inputDir, accel, brake, MaxSpeed);
        Player.SetRotate(inputDir, rotateTime);
    }
    #endregion

    private void Awake()
    {
        originWalkSpeed = walkSpeed;
        originSprintSpeed = sprintSpeed;
        originBrake = brake;

        InputManager.StartListeningInput(InputAction.Sprint, Sprint);
    }

    private void Sprint(InputAction action, float param) {
        if (!Player.Controller.Anim.GetBool(hash_bWalk)) return;
        Player.Controller.Anim.SetBool(hash_bSprint, true);
        MaxSpeed = sprintSpeed;
    }

    private void Stop() {
        if (Player.Controller.Anim.GetBool(hash_bSprint) && Player.CurSpeed > (walkSpeed + sprintSpeed) / 2) {
            Player.Controller.Anim.SetTrigger(hash_tStop);
            Player.LockInput(0.3f);
        }
        MaxSpeed = walkSpeed;
        Player.Controller.Anim.SetBool(hash_bSprint, false);
        Player.Controller.Anim.SetBool(hash_bWalk, false);
        Player.Decelerate(brake);
    }

    public void OnEnterOil()
    {
        ChangeWalkSpeed(oilWalkWeight, oilSprintWeight, 0.3f);
    }

    public void OnExitOil()
    {
        ChangeWalkSpeed(1f, 1f, 1f);
    }

    private void ChangeWalkSpeed(float walkW, float sprintW, float brakeW)
    {
        walkSpeed = originWalkSpeed * walkW;
        sprintSpeed = originSprintSpeed * sprintW;
        brake = originBrake * brakeW;

        if (Player.Controller.Anim.GetBool(hash_bWalk))
            MaxSpeed = walkSpeed;

        else if (Player.Controller.Anim.GetBool(hash_bSprint))
            MaxSpeed = walkSpeed;
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }
}
