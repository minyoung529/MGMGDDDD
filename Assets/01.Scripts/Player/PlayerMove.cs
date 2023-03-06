using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region 속력, 방향 관련 변수
    private Rigidbody rigid;
    [SerializeField] private float zoomMoveSpeed = 10;
    [SerializeField] private const float rotateTime = 2f;

    private float curSpeed = 0;
    public float CurSpeed => curSpeed;
    private Vector3 inputDir;
    private Vector3 forward;
    public Vector3 Forward
    {
        get
        {
            forward = MainCam.transform.forward;
            forward.y = 0;
            forward = forward.normalized;
            return forward;
        }
    }
    private Vector3 right;
    public Vector3 Right
    {
        get
        {
            right = MainCam.transform.right;
            right.y = 0;
            right = right.normalized;
            return right;
        }
    }
    #endregion

    #region 상태 관련 변수
    [SerializeField] private List<MoveState> stateList;
    private Dictionary<StateName, MoveState> stateDictionary = new Dictionary<StateName, MoveState>();
    [SerializeField] private MoveState curState;

    public bool isInputLock = false;
    public bool IsDecelerate;

    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region 애니메이션 관련 변수
    private Animator anim;
    public Animator Anim => anim;
    private int hash_iStateNum = Animator.StringToHash("iStateNum");
    private int hash_tStateChange = Animator.StringToHash("tStateChange");
    private int hash_fVertical = Animator.StringToHash("fVertical");
    private int hash_fHorizontal = Animator.StringToHash("fHorizontal");
    private int hash_fCurSpeed = Animator.StringToHash("fCurSpeed");
    #endregion

    private Camera mainCam;
    public Camera MainCam
    {
        get
        {
            if(mainCam==null)
                mainCam = Camera.main;
            return mainCam;
        }
    }


    [SerializeField] private float distanceToGround = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        mainCam = Camera.main;

        StartListen();
        SetStateDictionary();
    }

    private void SetStateDictionary() {
        foreach(MoveState item in stateList) {
            stateDictionary.Add(item.StateName, item);
        }
    }

    private void StartListen() {
        InputManager.StartListeningInput(InputAction.Move_Forward, (action, value) => GetInput(action, Forward));
        InputManager.StartListeningInput(InputAction.Back, (action, value) => GetInput(action, -Forward));
        InputManager.StartListeningInput(InputAction.Move_Right, (action, value) => GetInput(action, Right));
        InputManager.StartListeningInput(InputAction.Move_Left, (action, value) => GetInput(action, -Right));
        InputManager.StartListeningInput(InputAction.Zoom, (action, value) => {
            if (curState.StateName != StateName.Zoom)
                ChangeState(StateName.Zoom);
            else
                ChangeState(StateName.DefaultMove);
            });
        InputManager.StartListeningInput(InputAction.Jump, (action, value) => {
            if(CheckOnGround())
                ChangeState(StateName.Jump);
        });
    }
    private void GetInput(InputAction action, Vector3 input) {
        if (isInputLock) return;
        inputDir += input;
        inputDir = inputDir.normalized;
    }

    private void Update() {
        SendInput();
    }

    private void SendInput() {
        curState.OnInput(inputDir);
        anim.SetFloat(hash_fVertical, Vector3.Dot(Forward, inputDir));
        anim.SetFloat(hash_fHorizontal, Vector3.Dot(Right, inputDir));
        inputDir = Vector3.zero;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            //발 IK 위치 연산
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, 1 << Define.BOTTOM_LAYER))
            {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
            }

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, 1 << Define.BOTTOM_LAYER))
            {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.RightFoot, footposition);
            }
        }
    }

    public void ChangeState(StateName state) {
        MoveState targetState;
        if(!stateDictionary.TryGetValue(state, out targetState)) {
            Debug.LogError($"{state}에 해당하는 스테이트가 존재하지 않습니다");
            return;
        }
        curState.OnStateEnd(() => {
            curState = targetState;
            anim.SetInteger(hash_iStateNum, (int)state);
            anim.SetTrigger(hash_tStateChange);
            curState.OnStateStart();
        });
    }

    public void Accelerate(Vector3 inputDir, float accel = 2f, float brakeTime = 0.5f, float maxSpeed = 2f) {
        curSpeed += accel * Time.deltaTime;
        if (curSpeed > maxSpeed) {
            curSpeed = Mathf.MoveTowards(curSpeed, maxSpeed, curSpeed / brakeTime * Time.deltaTime);
            if(curSpeed < maxSpeed) {
                curSpeed = maxSpeed;
            }
        }
        Vector3 dir = inputDir * curSpeed;
        dir.y = rigid.velocity.y;
        rigid.velocity = dir;

        anim.SetFloat(hash_fCurSpeed, curSpeed);
    }

    public void Decelerate (float brakeTime = 0.5f) {
        curSpeed = Mathf.MoveTowards(curSpeed, 0, curSpeed / brakeTime * Time.deltaTime);
        if (curSpeed < 0) {
            curSpeed = 0;
        }
        Vector3 dir = inputDir * curSpeed;
        dir.y = rigid.velocity.y;

        if (dir.sqrMagnitude < 0.01f) return;
        rigid.velocity = dir;

        anim.SetFloat(hash_fCurSpeed, curSpeed);
    }
    public void SetRotate(Vector3 dir) {
        if (inputDir.sqrMagnitude <= 0) return;
        transform.forward = Vector3.RotateTowards(transform.forward, dir, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
    }
    public void SetRotate(Vector3 dir, float rotateTime = rotateTime) {
        if (inputDir.sqrMagnitude <= 0) return;
        transform.forward = Vector3.RotateTowards(transform.forward, dir, Vector3.Angle(transform.forward, inputDir) / rotateTime * Time.deltaTime, 0);
    }

    public bool CheckOnGround() {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position + Vector3.up * 0.2f, new Vector3(0.5f, 0, 0.5f), Vector3.down, out hit, Quaternion.identity, 0.3f, groundLayer)) {
            if (Vector3.Dot(Vector3.up, hit.normal) >= 0.4f) return true;
        }
        return false;
    }

    #region 애니메이션 이벤트
    public void JumpEvent() {
        if(curState is JumpState)
        {
            JumpState jump = (JumpState)curState;
            jump?.Jump();
        }
    }

    public void LandingEvent() {
        ChangeState(StateName.DefaultMove);
    }
    #endregion

    public void LockInput(float time)
    {
        StartCoroutine(LockTimer(time));
    }

    private IEnumerator LockTimer(float time)
    {
        isInputLock = true;
        yield return new WaitForSeconds(time);
        isInputLock = false;
    }

    public void ActiveRigidbody(bool isActive)
    {
        rigid.isKinematic = !isActive;
    }
}