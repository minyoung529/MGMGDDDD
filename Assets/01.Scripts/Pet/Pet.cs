using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Pet : MonoBehaviour, IFindable
{
    [SerializeField] protected PetTypeSO petInform;

    #region CheckList

    private bool isGet = false;
    private bool isCoolTime = false;
    private bool isSelected = false;
    protected bool isMouseMove = false;

    private bool isFindable = true;

    #endregion

    protected Rigidbody rigid;
    protected Collider coll;
    private Transform player;
    private Transform target;
    protected NavMeshAgent agent;

    private Vector3 originScale;

    [SerializeField] protected float sightRange = 5f;

    #region Get

    public bool IsGet => isGet;
    public bool CheckSkillActive { get { return (!isSelected || isCoolTime); } }
    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public Sprite petSprite => petInform.petUISprite;
    bool IFindable.IsFindable { get => isFindable & isGet; }

    #endregion

    private float distanceToPlayer = 5f;

    public Action onArrive { get; set; }

    private static bool isCameraAimPoint = true;
    public static bool IsCameraAimPoint
    {
        get => isCameraAimPoint;
        set => isCameraAimPoint = value;
    }

    protected virtual void Awake()
    {
        originScale = transform.localScale;

        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        ResetPet();
    }

    private void FixedUpdate()
    {
        if (!isGet) return;
        FollowTarget();
        CheckArrive();
        //LookAtPlayer();
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    #region Set

    protected virtual void ResetPet()
    {
        isCoolTime = false;
        agent.enabled = true;
        transform.localScale = originScale;
        agent.stoppingDistance = distanceToPlayer;
        SetTargetPlayer();
    }

    public void GetPet(Transform player)
    {
        isGet = true;
        this.player = player;

        SetTargetPlayer();
        StartListen();
        PetManager.Instance.AddPet(this);
    }
    public void LosePet()
    {
        ResetPet();
        StopListen();
        PetManager.Instance.DeletePet(this);
    }

    public void Select(bool select)
    {
        isSelected = select;
    }

    public void AgentEnabled(bool isEnabled)
    {
        agent.enabled = isEnabled;
    }
    #endregion

    #region Skill

    protected virtual void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;

        SkillDelay();
    }

    protected void SkillDelay()
    {
        isCoolTime = true;
        StartCoroutine(SkillCoolTime(petInform.skillDelayTime));
    }

    private IEnumerator SkillCoolTime(float t)
    {
        yield return new WaitForSeconds(t);
        isCoolTime = false;
    }

    protected virtual void SkillUp(InputAction inputAction, float value) {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    #endregion

    #region Move
    protected void LookAtPlayer()
    {
        Vector3 dir = target.position;
        dir = GameManager.Instance.GetCameraHit();

        Quaternion targetRot = Quaternion.LookRotation((dir - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    protected void FollowTarget() {
        if (!target) return;
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target, float stopDistance = 0, Action onArrive = null) {
        if (!isSelected) return;
        this.target = target;
        agent.stoppingDistance = stopDistance;
        if (!target) {
            StopNav(true);
            return;
        }
        this.onArrive = onArrive;
    }

    public void SetTargetPlayer() {
        if (!isSelected) return;
        target = player;
        agent.stoppingDistance = distanceToPlayer;
    }

    public void SetDestination(Vector3 target) {
        if (!isSelected) return;
        this.target = null;
        agent.SetDestination(target);
    }

    private void CheckArrive()
    {
        if (Vector3.Distance(agent.destination, transform.position) <= 1f)
        {
            onArrive?.Invoke();
            onArrive = null;
            OnArrive();
        }
    }

    protected virtual void OnArrive() { }

    public void StopNav(bool value) {
        agent.isStopped = value;
    }

    public void SetForcePosition(Vector3 position)
    {
        agent.enabled = false;
        transform.position = position;
        agent.enabled = true;
    }
    #endregion

    #region InputEvent
    public void MovePoint(InputAction inputAction, float value) {
        if (!isSelected) return;

        if (IsCameraAimPoint) {
            SetDestination(GameManager.Instance.GetCameraHit());
        }
        else {
            SetDestination(GameManager.Instance.GetMousePos());
        }

        transform.DOKill();
    }
    protected virtual void Withdraw(InputAction inputAction, float value) {
        ResetPet();
    }
    #endregion

    /// <summary>
    /// 맵에 존재하는 탐색 가능한 버튼을 찾음
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    private bool FindButton() {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, sightRange);
        if (target == null) return false;
        Vector3 dest = target.transform.position;
        agent.SetDestination(dest);
        return true;
    }

    #region Throw/Landing
    public virtual void OnThrow()
    {
        isFindable = false;
        StartCoroutine(LandingCoroutine());
    }

    private IEnumerator LandingCoroutine()
    {
        while (!CheckOnGround())
        {
            yield return null;
        }
        OnLanding();
    }
    public bool CheckOnGround()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, out hit, Quaternion.identity, 0.5f, 1 << Define.BOTTOM_LAYER))
        {
            if (Vector3.Dot(Vector3.up, hit.normal) >= 0.4f) return true;
        }
        return false;
    }

    public virtual void OnLanding()
    {
        agent.enabled = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        if (!FindButton())
            agent.SetDestination(transform.position);
        isFindable = true;
    }
    #endregion

    #region InputSystem
    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StartListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StartListeningInput(InputAction.Pet_Follow, Withdraw);
        InputManager.StartListeningInput(InputAction.Pet_Skill_Up, SkillUp);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StopListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StopListeningInput(InputAction.Pet_Follow, Withdraw);
        InputManager.StopListeningInput(InputAction.Pet_Skill_Up, SkillUp);
    }
    #endregion
}
