using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    #region ������Ʈ
    private Rigidbody rigid;
    private Animator anim;
    private Collider coll;

    public Rigidbody Rigid => rigid;
    public Animator Anim => anim;
    public Collider Coll => coll;
    #endregion

    #region �ӷ�, ���� ���� ����
    [SerializeField] private const float rotateTime = 2f;

    private float curSpeed = 0;
    public float CurSpeed => curSpeed;
    private Vector3 inputDir;
    private Vector3 forward;
    public Vector3 Forward {
        get {
            forward = MainCam.transform.forward;
            forward.y = 0;
            forward = forward.normalized;
            return forward;
        }
    }
    private Vector3 right;
    public Vector3 Right {
        get {
            right = MainCam.transform.right;
            right.y = 0;
            right = right.normalized;
            return right;
        }
    }
    #endregion

    #region ���� ���� ����
    [SerializeField] private List<MoveState> stateList;
    private Dictionary<StateName, MoveState> stateDictionary = new Dictionary<StateName, MoveState>();
    [SerializeField] private MoveState curState;

    public bool isInputLock = false;
    public bool IsDecelerate;

    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region �ִϸ��̼� ���� ����
    private int hash_iStateNum = Animator.StringToHash("iStateNum");
    private int hash_tStateChange = Animator.StringToHash("tStateChange");
    private int hash_fVertical = Animator.StringToHash("fVertical");
    private int hash_fHorizontal = Animator.StringToHash("fHorizontal");
    private int hash_fCurSpeed = Animator.StringToHash("fCurSpeed");
    #endregion

    #region Push And Pull

    public float pushPower = 2.0F;
    private bool isPushObj = false;
    private PushAndPull pushObj = null;

    #endregion

    private Camera mainCam;
    public Camera MainCam {
        get {
            if (mainCam == null)
                mainCam = Camera.main;
            return mainCam;
        }
    }

    [SerializeField] private float distanceToGround = 0;

    private Dictionary<InputAction, Action<InputAction, float>> actions = new Dictionary<InputAction, Action<InputAction, float>>();

    private void Awake() {
        mainCam = Camera.main;

        SetUpCompo();
        StartListen();
        SetUpStateDictionary();
    }

    private void SetUpCompo() {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }

    private void SetUpStateDictionary() {
        foreach (MoveState item in stateList) {
            stateDictionary.Add(item.StateName, item);
        }
    }
    private void StartListen() {
        actions.Add(InputAction.Move_Forward, (action, value) => GetInput(action, Forward));
        actions.Add(InputAction.Back, (action, value) => GetInput(action, -Forward));
        actions.Add(InputAction.Move_Right, (action, value) => GetInput(action, Right));
        actions.Add(InputAction.Move_Left, (action, value) => GetInput(action, -Right));
        actions.Add(InputAction.Zoom, (action, value) => {
            if (curState.StateName != StateName.Zoom)
                ChangeState(StateName.Zoom);
            else
                ChangeState(StateName.DefaultMove);
        });

        actions.Add(InputAction.Jump, (action, value) => {
            if (CheckOnGround())
                ChangeState(StateName.Jump);
        });

        foreach (var keyValue in actions) {
            InputManager.StartListeningInput(keyValue.Key, keyValue.Value);
        }
    }
    private void GetInput(InputAction action, Vector3 input) {
        if (isInputLock) return;
        inputDir += input;
        inputDir = inputDir.normalized;
    }

    /// <summary>
    /// �ִϸ��̼ǿ� ���� fHorizontal�� 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="length">������ ����:0~1</param>
    //private void SetAnimInput(Vector3 input, float length) {
    //    Anim.SetFloat(hash_fVertical, Vector3.Dot(Forward, inputDir));
    //    Anim.SetFloat(hash_fHorizontal, Vector3.Dot(Right, inputDir));
    //}

    private void Update() {
        SendInput();
    }

    private void SendInput() {
        curState.OnInput(inputDir);
        inputDir = Vector3.zero;
    }

    private void OnAnimatorIK(int layerIndex) {
        if (anim) {
            //�� IK ��ġ ����
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, 1 << Define.BOTTOM_LAYER)) {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
            }

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, 1 << Define.BOTTOM_LAYER)) {
                Vector3 footposition = hit.point;
                footposition.y += distanceToGround;
                anim.SetIKPosition(AvatarIKGoal.RightFoot, footposition);
            }
        }
    }

    public void ChangeState(StateName state) {
        MoveState targetState;
        if (!stateDictionary.TryGetValue(state, out targetState)) {
            Debug.LogError($"{state}�� �ش��ϴ� ������Ʈ�� �������� �ʽ��ϴ�");
            return;
        }
        curState.OnStateEnd(() => {
            curState = targetState;
            anim.SetInteger(hash_iStateNum, (int)state);
            anim.SetTrigger(hash_tStateChange);
            curState.OnStateStart();
        });
    }

    #region ���Ǽ� �Լ� (State���� �ַ� ���)
    public void Accelerate(Vector3 inputDir, float accel = 2f, float brakeTime = 0.5f, float maxSpeed = 2f) {
        curSpeed += accel * Time.deltaTime;
        if (curSpeed > maxSpeed) {
            curSpeed = Mathf.MoveTowards(curSpeed, maxSpeed, curSpeed / brakeTime * Time.deltaTime);
            if (curSpeed < maxSpeed) {
                curSpeed = maxSpeed;
            }
        }
        Vector3 dir = inputDir * curSpeed;
        dir.y = rigid.velocity.y;
        rigid.velocity = dir;

        anim.SetFloat(hash_fCurSpeed, curSpeed);
    }

    public void Decelerate(float brakeTime = 0.5f) {
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
        if (Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, out hit, Quaternion.identity, 1.3f, groundLayer)) {
            if (Vector3.Dot(Vector3.up, hit.normal) >= 0.4f) return true;
        }
        return false;
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.red;
    //    RaycastHit hit;
    //    bool col = Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(0.5f, 0.2f, 0.5f), Vector3.down, out hit, Quaternion.identity, 1.3f, groundLayer);
    //    Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
    //    Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down * 1.5f);

    //    if(col) {
    //        Gizmos.DrawWireCube(hit.point, new Vector3(0.5f, 0.2f, 0.5f));
    //    }
    //}

    public void LockInput(float time) {
        StartCoroutine(LockTimer(time));
    }

    private IEnumerator LockTimer(float time) {
        isInputLock = true;
        yield return new WaitForSeconds(time);
        isInputLock = false;
    }
    #endregion

    #region �ִϸ��̼� �̺�Ʈ
    public void JumpEvent() {
        if (curState is JumpState) {
            JumpState jump = (JumpState)curState;
            jump?.Jump();
        }
    }

    public void LandingEvent() {
        ChangeState(StateName.DefaultMove);
    }
    #endregion


    public void ActiveRigidbody(bool isActive) {
        rigid.isKinematic = !isActive;
    }

    private void OnDestroy() {
        foreach (var keyValue in actions) {
            InputManager.StopListeningInput(keyValue.Key, keyValue.Value);
        }
    }
}