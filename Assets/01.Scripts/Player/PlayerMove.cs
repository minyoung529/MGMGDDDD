using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region 속력, 방향 관련 변수
    [SerializeField] private float walkSpeed = 100;
    [SerializeField] private float sprintSpeed = 10;
    [SerializeField] private float zoomMoveSpeed = 10;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float rotateTime = 1;
    [SerializeField] private float decelPower = 10;
    private float curSpeed = 0;

    private Rigidbody rigid;

    private Vector3 inputDir;
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
    private bool isInputLock = false;
    private bool isZoom = false;
    private bool isSprint = false;
    #endregion

    #region 애니메이션 관련 변수
    private Animator anim;
    private int stopHash = Animator.StringToHash("stop");
    private int walkHash = Animator.StringToHash("walk");
    private int sprintHash = Animator.StringToHash("sprint");
    private int zoomHash = Animator.StringToHash("zoom");
    #endregion

    private Camera mainCam;

    [SerializeField] private float distanceToGround = 0;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        mainCam = Camera.main;

        curSpeed = walkSpeed;
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
        InputManager.StartListeningInput(InputAction.Jum, InputType.GetKeyDown, Jump);
    }

    private void Update() {
        SetVelocity();
        SetRotate();
        ResetInput();
        Decelerate();
    }

    private void OnAnimatorIK(int layerIndex) {
        if (anim) {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, Define.BOTTOM_LAYER)) {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
            }
        }
    }

    private void GetInput(Vector3 input) {
        if (isInputLock) return;
        inputDir += input;
        inputDir = inputDir.normalized;
        inputTimer = inputDeadTime;
        anim.SetBool(walkHash, true);
        if (!isZoom) return;
        anim.SetFloat("horizontal", Vector3.Dot(Right, inputDir));
        anim.SetFloat("vertical", Vector3.Dot(Forward, inputDir));
    }

    private void ResetInput() {
        if(inputTimer <= 0) {
            if(isSprint) {
                anim.SetTrigger(stopHash);
                LockInput(0.125f);
            }
            isSprint = false;
            anim.SetBool(sprintHash, false);
            anim.SetBool(walkHash, false);
            inputDir = Vector3.zero;
            return;
        }
        inputTimer -= Time.deltaTime;
    }

    private void SetVelocity() {
        rigid.velocity = inputDir * curSpeed;
    }

    private void SetRotate() {
        if (!isZoom && inputDir.sqrMagnitude > 0) {
            transform.forward = Vector3.RotateTowards(transform.forward, inputDir, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
            return;
        }
        else if (isZoom) {
            transform.forward = Vector3.RotateTowards(transform.forward, Forward, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
        }
    }

    private void Decelerate() {
        if (inputDir.sqrMagnitude > 0) return;
        rigid.velocity = Vector3.MoveTowards(rigid.velocity, Vector3.zero, decelPower);
    }

    private void Sprint(InputAction action, InputType type, float value) {
        if (isInputLock) return;
        isSprint = !isSprint;
        curSpeed = isSprint ? sprintSpeed : walkSpeed;
        anim.SetBool(sprintHash, isSprint);
    }

    private void Zoom(InputAction action, InputType type, float value) {
        if (isInputLock) return;
        isZoom = !isZoom;
        anim.SetBool(zoomHash, isZoom);
    }

    private void Jump(InputAction action, InputType type, float value) {

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