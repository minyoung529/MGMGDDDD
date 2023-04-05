using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Pet : MonoBehaviour
{
    [SerializeField] protected PetTypeSO petInform;
    [SerializeField] protected float sightRange = 5f;
    [SerializeField] protected float collRadius = 0.7f;

    #region CheckList

    private bool isCoolTime = false;
    protected bool isMouseMove = false;
    private bool isInputLock = false;
    public bool IsInputLock { get { return isInputLock; } set { isInputLock = value; } }

    #endregion

    protected Rigidbody rigid;
    protected Collider coll;
    protected Transform player;
    protected Transform target;
    protected NavMeshAgent agent;
    protected PetThrow petThrow; 

    private Vector3 originScale;


    #region Get

    public bool IsCoolTime => isCoolTime;
    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public PetThrow PetThrow => petThrow;
    public Sprite petSprite => petInform.petUISprite;

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
        petThrow = GetComponent<PetThrow>();
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
        if (!target || !agent.isOnNavMesh) return;
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target, float stopDistance = 0, Action onArrive = null) {
        rigid.velocity = Vector3.zero;
        this.target = target;
        agent.stoppingDistance = stopDistance;
        if (!target) {
            agent.ResetPath();
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

    public void SetDestination(Vector3 target, float stopDistance = 0, Action onArrive = null) {
        if (!agent.isOnNavMesh) return;
        this.onArrive = onArrive;
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

    public void SetForcePosition(Vector3 position)
    {
        agent.enabled = false;
        transform.position = position;
        agent.enabled = true;
    }
    #endregion

    #region Nav_Get/Set
    public void SetNavIsStopped(bool value) {
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value) {
        agent.enabled = value;
    }
    public bool GetIsOnNavMesh() {
        return agent.isOnNavMesh;
    }
    public Vector3 GetDestination() {
        return agent.destination;
    }
    #endregion

    #region InputEvent
    public void MovePoint() {
        if (isInputLock) return;
        Debug.Log("Click");
        if (IsCameraAimPoint) {
            SetDestination(GameManager.Instance.GetCameraHit());
        }
        else {
            SetDestination(GameManager.Instance.GetMousePos());
        }

        //transform.DOKill();
    }

    public virtual void Withdraw() {
        if (isInputLock) return;
        ResetPet();
    }
    #endregion

    #region AI
    /// <summary>
    /// 맵에 존재하는 탐색 가능한 버튼을 찾음
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    public bool FindButton() {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, sightRange);
        if (!target) return false;
        Vector3 dest = target.transform.position;
        agent.SetDestination(dest);
        return true;
    }
    #endregion
}
