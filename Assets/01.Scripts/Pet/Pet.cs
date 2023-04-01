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
    protected PetHold hold; 

    private Vector3 originScale;


    #region Get

    public bool IsCoolTime => isCoolTime;
    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public PetHold Hold => hold;
    public Sprite petSprite => petInform.petUISprite;
    public PetType GetPetType => petInform.petType;

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

    public void SetDestination(Transform target)
    {
        SetTarget(target);
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

    public void SetNavIsStopped(bool value) {
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value) {
        agent.enabled = value;
    }
    public Vector3 GetDestination() {
        return agent.destination;
    }
    public void ResetNav()
    {
        agent.ResetPath();
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
        if (isInputLock) return;

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

    /// <summary>
    /// �ʿ� �����ϴ� Ž�� ������ ��ư�� ã��
    /// </summary>
    /// <returns>Ž�� ���� ����</returns>
    private bool FindButton() {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, sightRange);
        if (target == null) return false;
        Vector3 dest = target.transform.position;

        try
        {
            agent.SetDestination(dest);
        }
        catch(Exception e)
        {
            Debug.Log("PATH�� �����ϴ�.");
        }
        return true;
    }

    #region Throw/Landing
    public virtual void OnThrow()
    {
        StartCoroutine(LandingCoroutine());
    }

    private IEnumerator LandingCoroutine()
    {
        int t = 0;
        while (!CheckCollision())
        {
            t++;
            yield return null;
        }
        OnLanding();
    }

    public bool CheckCollision()
    {
        if (Physics.OverlapSphere(transform.position, collRadius, 1 << Define.BOTTOM_LAYER).Length > 0)
            return true;
        return false;
    }

    public virtual void OnLanding() 
    {
        SetNavEnabled(true);
        coll.enabled = true;
        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        isInputLock = false;
        if (!FindButton())
            SetTarget(null);
    }
    #endregion
}
