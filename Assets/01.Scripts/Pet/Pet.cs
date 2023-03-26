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

    private bool isCoolTime = false;
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

    public bool IsCoolTime => isCoolTime;
    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public Sprite petSprite => petInform.petUISprite;
    bool IFindable.IsFindable { get => isFindable; }

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

    public virtual void OnUpdate() 
    {
        CheckArrive();
        FollowTarget();
    }

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
        this.player = player;

        SetTargetPlayer();
        PetManager.Instance.AddPet(this);
    }
    public void LosePet()
    {
        ResetPet();
        PetManager.Instance.DeletePet(this);
    }

    public void AgentEnabled(bool isEnabled)
    {
        agent.enabled = isEnabled;
    }
    #endregion

    #region Skill

    public virtual void Skill()
    {
        if (isCoolTime) return;
        SkillDelay();
    }

    private void SkillDelay()
    {
        isCoolTime = true;
        StartCoroutine(SkillCoolTime(petInform.skillDelayTime));
    }

    private IEnumerator SkillCoolTime(float t)
    {
        yield return new WaitForSeconds(t);
        isCoolTime = false;
    }

    public virtual void SkillUp() {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    #endregion

    #region Move

    private void FollowTarget() {
        if (!target) return;
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target, float stopDistance = 0, Action onArrive = null) {
        rigid.velocity = Vector3.zero;
        this.target = target;
        agent.stoppingDistance = stopDistance;
        if (!target) {
            agent.ResetPath();
            SetNavIsStopped(true);
            return;
        }

        SetNavEnabled(true);
        SetNavIsStopped(false);
        this.onArrive = onArrive;
    }

    public void SetTargetPlayer() {
        SetNavEnabled(true);
        SetNavIsStopped(false);
        rigid.velocity = Vector3.zero;
        target = player;
        agent.stoppingDistance = distanceToPlayer;
    }

    public void SetDestination(Vector3 target, float stopDistance = 0) {
        SetNavEnabled(true);
        SetNavIsStopped(false);
        rigid.velocity = Vector3.zero;
        this.target = null;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(target);
    }

    private void CheckArrive() {
        if (Vector3.Distance(agent.destination, transform.position) <= 1f) {
            onArrive?.Invoke();
            onArrive = null;
        }
    }

    public void SetNavIsStopped(bool value) {
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value) {
        agent.enabled = value;
    }
    public Vector3 GetDestination() {
        return agent.destination;
    }

    public void SetForcePosition(Vector3 position)
    {
        agent.enabled = false;
        transform.position = position;
        agent.enabled = true;
    }
    #endregion

    #region InputEvent
    public void MovePoint() {
        if (IsCameraAimPoint) {
            SetDestination(GameManager.Instance.GetCameraHit());
        }
        else {
            SetDestination(GameManager.Instance.GetMousePos());
        }

        //transform.DOKill();
    }

    public virtual void Withdraw() {
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
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.1f, 0.5f), Vector3.down, out hit, Quaternion.identity, 0.4f, 1 << Define.BOTTOM_LAYER))
        {
            if (Vector3.Dot(Vector3.up, hit.normal) >= 0.4f) return true;
        }
        return false;
    }

    public virtual void OnLanding() 
    {
        SetNavEnabled(true);
        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        if (!FindButton())
            agent.SetDestination(transform.position);
        isFindable = true;
    }
    #endregion
}
