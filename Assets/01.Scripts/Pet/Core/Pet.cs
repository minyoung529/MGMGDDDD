using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour, IThrowable
{
    [SerializeField] public PetTypeSO petInform;
    [SerializeField] private PetEmotion emotion;
    private Vector3 originScale;
    private float originalAgentSpeed;
    private float interactRadius = 4.5f;

    private ChangePetEmission emission;
    public ChangePetEmission Emission => emission;

    public PetEmotion Emotion => emotion;

    #region Property

    protected Transform target;
    protected Transform player;
    public Transform Player => player;
    public Transform Target => target;
    public Vector3 destination;
    private readonly float distanceToPlayer = 5f;

    #endregion

    #region CheckList

    private bool isCoolTime = false;
    private bool skilling = false;
    public bool Skilling { get { return skilling; } set { skilling = value; } }
    protected bool isMouseMove = false;
    private bool isMovePointLock = false;
    public bool IsMovePointLock { get => isMovePointLock; set => isMovePointLock = value; }

    #endregion

    #region Component
    protected Collider coll;
    protected Rigidbody rigid;
    protected NavMeshAgent agent;
    private float beginAcceleration;
    public float AgentAcceleration
    {
        get => agent.acceleration;
        set { agent.acceleration = value; }
    }
    #endregion

    #region Get

    public Vector3 MouseUpDestination { get; private set; }
    public Rigidbody Rigid => rigid;
    public Collider Coll => coll;
    public NavMeshAgent Agent => agent;
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
        emission = GetComponentInChildren<ChangePetEmission>();

        AxisController = new AxisController(transform);
        beginAcceleration = agent.acceleration;

        originScale = transform.localScale;
        originalAgentSpeed = agent.speed;

        PetState[] compos = stateParent.GetComponents<PetState>();
        PetState[] states = new PetState[(int)PetStateName.Length];
        foreach (PetState item in compos)
        {
            states[(int)item.StateName] = item;
            states[(int)item.StateName].SetUp(transform);
        }
        stateMachine = new StateMachine<Pet>(this, states);
    }


    public virtual void OnUpdate()
    {
        State.OnUpdate();
        Chase();

        if (agent.isOnOffMeshLink)
        {
            agent.speed = originalAgentSpeed * 0.5f;
        }
        else
        {
            agent.speed = originalAgentSpeed;
        }

    }

    #region Set

    public virtual void ResetPet()
    {
        isCoolTime = false;
        skilling = false;
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
    #endregion

    #region Skill

    public virtual void Skill()
    {
        if (isCoolTime) return;
        skilling = true;
        SkillDelay();
        State.ChangeState((int)PetStateName.Skill);
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
        skilling = false;
    }

    public virtual void SkillUp()
    {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    #endregion

    #region Move
    public void SetTarget(Transform target, float stopDistance = 0)
    {
        if (agent == null) return;
        this.target = target;
        agent.stoppingDistance = stopDistance;

        if (!target && agent.enabled)
        {
            agent.ResetPath();
            return;
        }
    }
    
    public void SetTargetNull()
    {
        if (agent == null) return;
        target = null;

        if (agent.enabled)
        {
            agent.ResetPath();
            return;
        }
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

    public void SetDestination(Vector3 target)
    {
        if (!agent.isOnNavMesh) return;
        rigid.velocity = Vector3.zero;
        destination = AxisController.CalculateDestination(target);
        Event.TriggerEvent((int)PetEventName.OnSetDestination);
    }

    public void SetDestination(Vector3 target, float stopDistance)
    {
        if (!agent.isOnNavMesh) return;
        rigid.velocity = Vector3.zero;
        agent.stoppingDistance = stopDistance;
        destination = AxisController.CalculateDestination(target);
        Event.TriggerEvent((int)PetEventName.OnSetDestination);
    }

    private void Chase()
    {
        if (!target) return;
        if (Vector3.Distance(GetNearestNavMeshPosition(transform.position), GetNearestNavMeshPosition(target.position))
            >= agent.stoppingDistance)
        {
            SetDestination(target.position);
        }
    }
    #endregion

    #region Nav_Get/Set
    public void SetNavIsStopped(bool value)
    {
        if (agent == null) return;
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value)
    {
        if (agent == null) return;
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
    public void MovePoint()
    {
        if (IsMovePointLock) return;

        if (IsCameraAimPoint)
        {
            destination = GameManager.Instance.GetCameraHit();
        }
        else
        {
            destination = GameManager.Instance.GetMousePos();
        }

        SetTarget(null);
        SetDestination(destination);
    }

    #endregion

    public void ResetAgentValue()
    {
        agent.acceleration = beginAcceleration;
    }

    #region Interact

    public void InteractionPoint()
    {
        if (SelectedObject.CurInteractObject) return;

        SelectedObject.SetInteractionObject();
        Event.StartListening((int)PetEventName.OnArrive, CheckInteract);
    }

    private void CheckInteract()
    {
        Event.StopListening((int)PetEventName.OnArrive, CheckInteract);
        
        SelectedObject.CurInteractObject.OnInteract();
        Event.TriggerEvent((int)PetEventName.OnInteractEnd);
    }

    public void SetInteractNull()
    {
        SelectedObject.CurInteractObject = null;
    }

    #endregion

    private void OnDisable()
    {
        State.OnDisable();
    }

    #region IThrowable
    public void OnHold() {
        Rigid.velocity = Vector3.zero;
        Rigid.isKinematic = true;
        Coll.enabled = false;
        SetNavEnabled(false);
    }

    public void OnDrop() {
        Coll.enabled = true;
        Rigid.isKinematic = false;
        Rigid.velocity = Vector3.zero;
        SetNavEnabled(true);
    }

    public void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse) {
        Rigid.velocity = Vector3.zero;
        Rigid.isKinematic = false;
        Rigid.AddForce(force, forceMode);
        Event.TriggerEvent((int)PetEventName.OnThrew);
    }

    public void OnThrow() {
        Coll.enabled = true;
        Rigid.constraints = RigidbodyConstraints.None;
    }

    public void OnLanding() {
        Rigid.constraints = RigidbodyConstraints.FreezeAll &~ RigidbodyConstraints.FreezePositionY;
        Rigid.velocity = Vector3.zero;
    }
    #endregion
}