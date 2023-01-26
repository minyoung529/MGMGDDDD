using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region 속력, 방향 관련 변수
    private Rigidbody rigid;

    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float sprintSpeed = 10;
    [SerializeField] private float zoomMoveSpeed = 10;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float rotateTime = 1;
    [SerializeField] private float accelPower = 10;
    [SerializeField] private float decelPower = 10;
    private float maxSpeed = 0;
    private float curSpeed = 0;

    private Vector3 inputDir;
    private Vector3 beforeInput = Vector3.zero;
    private float inputDeadTime = 0.05f;
    private float inputTimer = 0;

    private Vector3 forward;
    private Vector3 Forward { 
        get {
            forward = mainCam.transform.forward;
            forward.y = 0;
            return forward; 
        } 
    }
    private Vector3 right;
    private Vector3 Right {
        get {
            right = mainCam.transform.right;
            right.y = 0;
            return right;
        }
    }
    #endregion

    #region 상태 관련 변수
    private bool  isInputLock = false;
    #endregion

    #region 애니메이션 관련 변수
    private Animator anim;
    private int stopHash = Animator.StringToHash("stop");
    private int walkHash = Animator.StringToHash("walk");
    private int sprintHash = Animator.StringToHash("sprint");
    private int zoomHash = Animator.StringToHash("zoom");
    private int jumpHash = Animator.StringToHash("jump");
    private int horizontalHash = Animator.StringToHash("horizontal");
    private int verticalHash = Animator.StringToHash("vertical");
    #endregion

    private Camera mainCam;

    [SerializeField] private float distanceToGround = 0;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        mainCam = Camera.main;

        maxSpeed = walkSpeed;
    }

    private void Start() {
        StartListen();
    }

    private void StartListen() {
        InputManager.StartListeningInput(InputAction.Move_Forward, InputType.GetKey, (action, type, value) => GetInput(Forward));
        InputManager.StartListeningInput(InputAction.Back, InputType.GetKey, (action, type, value) => GetInput(-Forward));
        InputManager.StartListeningInput(InputAction.Move_Right, InputType.GetKey, (action, type, value) => GetInput(Right));
        InputManager.StartListeningInput(InputAction.Move_Left, InputType.GetKey, (action, type, value) => GetInput(-Right));
        InputManager.StartListeningInput(InputAction.Zoom, InputType.GetKeyDown, Zoom);
        InputManager.StartListeningInput(InputAction.Sprint, InputType.GetKeyDown, Sprint);
        InputManager.StartListeningInput(InputAction.Jump, InputType.GetKeyDown, Jump);
    }

    private void Update() {
        SetRotate();
        ResetInput();
        Decelerate();
    }

    private void OnAnimatorIK(int layerIndex) {
        if (anim) {
            //발 IK 위치 연산
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, Define.BOTTOM_LAYER)) {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
            }

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, Define.BOTTOM_LAYER)) {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.RightFoot, footposition);
            }
        }
    }

    private void GetInput(Vector3 input) {
        if (isInputLock) return;
        inputDir += input;
        inputDir = inputDir.normalized;
        inputTimer = inputDeadTime;
        anim.SetBool(walkHash, true);
        Accelerate();

        if (!anim.GetBool(zoomHash)) return;
        anim.SetFloat(horizontalHash, Vector3.Dot(right, inputDir) * curSpeed);
        anim.SetFloat(verticalHash, Vector3.Dot(forward, inputDir) * curSpeed);
    }

    private void Accelerate() {
        if (Vector3.Dot(inputDir, beforeInput) < 0) curSpeed = 0;
        if (curSpeed < maxSpeed) {
            curSpeed += accelPower * Time.deltaTime;
        }
        else {
            curSpeed = maxSpeed;
        }
        rigid.velocity = inputDir * curSpeed;
        beforeInput = inputDir;
    }

    private void ResetInput() {
        if(inputTimer <= 0) {
            if(anim.GetBool(sprintHash)) {
                anim.SetTrigger(stopHash);
                LockInput(0.125f);
            }
            anim.SetBool(sprintHash, false);
            anim.SetBool(walkHash, false);
            inputDir = Vector3.zero;
            beforeInput = Vector3.zero;
            return;
        }
        inputTimer -= Time.deltaTime;
    }

    private void SetRotate() {
        if (inputDir.sqrMagnitude <= 0) return;
        if (!anim.GetBool(zoomHash))
            transform.forward = Vector3.RotateTowards(transform.forward, inputDir, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
        else
            transform.forward = Vector3.RotateTowards(transform.forward, Forward, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
    }

    private void Decelerate() {
        if (inputDir.sqrMagnitude > 0) return;
        rigid.velocity = Vector3.MoveTowards(rigid.velocity, Vector3.zero, decelPower);
        if (curSpeed > 0)
            curSpeed -= decelPower * Time.deltaTime;
        else
            curSpeed = 0;
    }

    private void Sprint(InputAction action, InputType type, float value) {
        if (isInputLock || anim.GetBool(zoomHash)) return;
        anim.SetBool(sprintHash, !anim.GetBool(sprintHash));
        maxSpeed = anim.GetBool(sprintHash) ? sprintSpeed : walkSpeed;
    }

    private void Zoom(InputAction action, InputType type, float value) {
        if (isInputLock) return;
        maxSpeed = zoomMoveSpeed;
        anim.SetBool(sprintHash, false);
        anim.SetBool(zoomHash, !anim.GetBool(zoomHash));
    }

    private void Jump(InputAction action, InputType type, float value) {
        anim.SetTrigger(jumpHash);
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    public void LockInput(float time) {
        StartCoroutine(LockTimer(time));
    }

    private IEnumerator LockTimer(float time) {
        isInputLock = true;
        yield return new WaitForSeconds(time);
        isInputLock = false;
    }
}