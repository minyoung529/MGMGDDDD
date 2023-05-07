using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    public PetTypeSO petInform;
    
    private Vector3 originScale;
    private float originalAgentSpeed;
    private ChangePetEmission emission;
    public ChangePetEmission Emission => emission;

    #region 이동관련

    protected Transform target;
    protected Transform player;
    public Transform Player => player;
    private readonly float distanceToPlayer = 5f;
    public Action OnArrive { get; set; }

    #endregion

    #region CheckList

    private bool isCoolTime = false;
    private bool skilling = false;
    public bool Skilling { get { return skilling; } set { skilling = value; } }
    protected bool isMouseMove = false;
    private bool isMovePointLock = false;
    public bool IsMovePointLock { get => isMovePointLock; set => isMovePointLock = value; }
    private bool isInputLock = false;
    public bool IsInputLock { get { return isInputLock; } set { isInputLock = value; } }

    #endregion

    #region Component
    protected Collider coll;
    protected Rigidbody rigid;
    protected NavMeshAgent agent;
    protected PetThrow petThrow;
    private float beginAcceleration;
    public float AgentAcceleration
    {
        get => agent.acceleration;
        set { agent.acceleration = value; }
    }
    #endregion

    #region Get

    public bool IsInteraction { get; set; }
    public bool IsCoolTime => isCoolTime;
    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public PetThrow PetThrow => petThrow;
    public Sprite petSprite => petInform.petUISprite;
    public PetType GetPetType => petInform.petType;
    public Color petColor => petInform.outlineColor;

    #endregion

    private static bool isCameraAimPoint = true;
    public static bool IsCameraAimPoint
    {
        get => isCameraAimPoint;
        set => isCameraAimPoint = value;
    }

    public AxisController AxisController { get; set; }

    [SerializeField] private Transform stateParent = null;
    private StateMachine<Pet> stateMachine;
    public StateMachine<Pet> State => stateMachine;
    private LocalEvent petEvent = new LocalEvent();
    public LocalEvent Event => petEvent;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
        petThrow = GetComponent<PetThrow>();
        emission = GetComponentInChildren<ChangePetEmission>();

        AxisController = new AxisController(transform);
        beginAcceleration = agent.acceleration;

        originScale = transform.localScale;
        originalAgentSpeed = agent.speed;

        PetState[] compos = stateParent.GetComponents<PetState>();
        PetState[] states = new PetState[(int)PetStateName.Length];
        foreach (PetState item in compos) {
            states[(int)item.StateName] = item;
            states[(int)item.StateName].SetUp(transform);
        }
        stateMachine = new StateMachine<Pet>(this, states);
    }

    private void Start()
    {
        ResetPet();
    }

    public virtual void OnUpdate()
    {
        State.OnUpdate();
        CheckArrive();
        FollowTarget();

        if(agent.isOnOffMeshLink)
        {
            agent.speed = originalAgentSpeed * 0.5f;
        }
        else
        {
            agent.speed = originalAgentSpeed;
        }
    }

    #region Set

    protected virtual void ResetPet()
    {
        StopSkill();
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
        if (IsInputLock) return;
        skilling = true;
        SkillDelay();
    }

    private void SkillDelay()
    {
        StartCoroutine(SkillCoolTime(petInform.skillDelayTime));
    }

    private IEnumerator SkillCoolTime(float t)
    {
        isCoolTime = true;
        yield return new WaitForSeconds(t);
        isCoolTime = false;
    }

    public virtual void SkillUp()
    {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    #endregion

    #region Move

    private void FollowTarget()
    {
        if (!target || !agent.enabled || !agent.isOnNavMesh) return;
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target, float stopDistance = 0, Action onArrive = null)
    {
        this.target = target;
        agent.stoppingDistance = stopDistance;

        if (!target)
        {
            agent.ResetPath();
            return;
        }

        this.OnArrive = onArrive;
    }

    public void SetDestination(Transform target)
    {
        SetTarget(target);
    }

    public void SetTargetPlayer()
    {
        target = player;
        agent.stoppingDistance = distanceToPlayer;
    }

    public void SetPlayerTransform(Transform player)
    {
        this.player = player;
    }

    public virtual void StopSkill()
    {
        skilling = false;
    }

    public void SetDestination(Vector3 target, float stopDistance = 0, Action onArrive = null)
    {
        if (!agent.isOnNavMesh) return;
        this.OnArrive = onArrive;
        rigid.velocity = Vector3.zero;
        this.target = null;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(AxisController.CalculateDestination(target));
    }

    private void CheckArrive()
    {
        if (Vector3.Distance(agent.destination, transform.position) <= 1f)
        {
            OnArrive?.Invoke();
            OnArrive = null;
        }
    }
    #endregion

    #region Nav_Get/Set
    public void SetNavIsStopped(bool value)
    {
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value)
    {
        agent.enabled = value;
    }
    public bool GetIsOnNavMesh()
    {
        return agent.isOnNavMesh;
    }
    public Vector3 GetDestination()
    {
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
    public Vector3 GetNearestNavMeshPosition(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return position;
        }
    }
    #endregion

    #region InputEvent
    public void MovePoint(bool selected = false)
    {
        if (isInputLock || IsMovePointLock) return;

        if (selected)
        {
            InteractionPoint();
            return;
        }

        if (IsCameraAimPoint)
        {
            SetDestination(GameManager.Instance.GetCameraHit());
        }
        else
        {
            SetDestination(GameManager.Instance.GetMousePos());
        }

        //transform.DOKill();
    }

    public virtual void InteractionPoint()
    {

    }

    public virtual void Withdraw()
    {
        if (isInputLock) return;
        ResetPet();
    }
    #endregion

    public void ResetAgentValue()
    {
        agent.acceleration = beginAcceleration;
    }

    private void OnDisable() {
        State.OnDisable();
    }
}