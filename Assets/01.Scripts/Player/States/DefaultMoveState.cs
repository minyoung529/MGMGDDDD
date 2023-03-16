using UnityEngine;

public class DefaultMoveState : MoveState
{
    #region abstarct 구현 부분
    public override StateName StateName => StateName.DefaultMove;

    private PlayerMove player = null;
    public override PlayerMove PlayerMove => player;

    public override void OnInput(Vector3 inputDir)
    {
        if (inputDir.sqrMagnitude <= 0)
        {
            Stop();
            return;
        }
        player.Anim.SetBool(hash_bWalk, true);
        player.Accelerate(inputDir, accel, brake, MaxSpeed);
        player.SetRotate(inputDir, rotateTime);
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

    private float originSprintSpeed;
    private float originWalkSpeed;
    private float originBrake;

    private bool isSlide = false;
    #endregion

    #region 애니메이션
    private int hash_bWalk = Animator.StringToHash("bWalk");
    private int hash_bSprint = Animator.StringToHash("bSprint");
    private int hash_tStop = Animator.StringToHash("tStop");
    #endregion

    private void Awake()
    {
        player = GetComponent<PlayerMove>();

        originWalkSpeed = walkSpeed;
        originSprintSpeed = sprintSpeed;
        originBrake = brake;

        InputManager.StartListeningInput(InputAction.Sprint, Sprint);
    }

    private void Sprint(InputAction action, float param)
    {
        if (!player.Anim.GetBool(hash_bWalk)) return;
        player.Anim.SetBool(hash_bSprint, true);
        MaxSpeed = sprintSpeed;
    }

    private void Stop()
    {
        if (player.Anim.GetBool(hash_bSprint) && player.CurSpeed > (walkSpeed + sprintSpeed) / 2)
        {
            player.Anim.SetTrigger(hash_tStop);
            player.LockInput(0.3f);
        }
        MaxSpeed = walkSpeed;
        player.Anim.SetBool(hash_bSprint, false);
        player.Anim.SetBool(hash_bWalk, false);
        player.Decelerate(brake);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isSlide && other.CompareTag(Define.OIL_BULLET_TAG))
        {
            isSlide = true;
            walkSpeed = originWalkSpeed * 2f;
            sprintSpeed = originSprintSpeed * 2f;
            brake = originBrake * 0.3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isSlide && other.CompareTag(Define.OIL_BULLET_TAG))
        {
            isSlide = false;
            walkSpeed = originWalkSpeed;
            sprintSpeed = originSprintSpeed;
            brake = originBrake;
        }
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }
}
