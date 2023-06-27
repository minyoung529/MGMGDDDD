using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public abstract class Pet : MonoBehaviour, IThrowable
{
    [SerializeField] public PetTypeSO petInform;

    private Vector3 originScale;
    private float originalAgentSpeed;
    private float interactRadius = 4.5f;

    private ChangePetEmission emission;
    public ChangePetEmission Emission => emission;


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

    #region Ping

    [SerializeField] private GameObject pingPrefab;
    private IObjectPool<Ping> pingPool;
    public bool PosDown { get; set; }

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
    private Ping curPing;


    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
        emission = GetComponentInChildren<ChangePetEmission>();
        pingPool = new ObjectPool<Ping>(CreatePing, OnGetPing, OnReleasePing, OnDestroyedPing, maxSize: 20);

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

        CutSceneManager.Instance?.AddStartCutscene(Pause);
    }

    void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(Pause);
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

    public void Pause()
    {
        State.ChangeState((int)PetStateName.Pause);
    }

    public void StopPause()
    {
        State.ChangeState(State.BeforeStateIndex);
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
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
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
        if (!target || !GetIsOnNavMesh()) return;

        if (!IsTargetOnRoute(target))
        {
            SetTargetNull();
            return;
        }
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
        if (!agent.enabled) return;
        agent.isStopped = value;
    }
    public void SetNavEnabled(bool value)
    {
        if (agent == null) return;
        agent.enabled = value;
    }
    public bool GetIsOnNavMesh()
    {
        if (!agent.enabled) return false;
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
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.enabled = false;
        transform.position = position;
        agent.enabled = true;
    }
    public Vector3 GetNearestNavMeshPosition(Vector3 position, float maxDistance = 5f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return position;
        }
    }

    public bool IsTargetOnRoute(Transform target)
    {
        if (!GetIsOnNavMesh()) return false; //�׺�޽� ������
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, GetNearestNavMeshPosition(target.position, 20f), NavMesh.AllAreas, path); //��ΰ� �׷�������
        if (path.corners.Length < 1) return false;
      //  if (Vector3.Distance(target.position, path.corners[path.corners.Length - 1]) > 5f) return false; //����� �������� �÷��̾� ��ó����
        if (Vector3.Distance(target.position, path.corners[path.corners.Length - 1]) > 8f) return false; //����� �������� �÷��̾� ��ó����
        return true;
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
        SetPing(destination);
        SetTarget(null);
        SetDestination(destination);
    }

    #endregion

    #region Ping
    #region POOL
    int index = 0;
    private Ping CreatePing()
    {
        Ping obj = Instantiate(pingPrefab).transform.GetComponent<Ping>();
        obj.name = pingPrefab.name + "_"+index;
        obj.transform.position = transform.position;
        index++;
        return obj;
    }

    public void OnGetPing(Ping obj)
    {
        obj.InitPing(this);
        obj.gameObject.SetActive(true);
        obj.PingEffect();
    }

    public void OnReleasePing(Ping obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void OnDestroyedPing(Ping obj)
    {
        Destroy(obj.gameObject);
    }
    #endregion

    public void SetPing(Vector3 destination)
    {
        if(curPing == null)
        {
            curPing = pingPool.Get();
        }
        curPing.SetPoint(destination);
    }
    public void StopPing()
    {
        if (curPing == null) return;
        curPing.OffPoint();
        curPing = null;
    }
    public void ReleasePing(Ping ping)
    {
        if(ping.gameObject.activeSelf) pingPool.Release(ping);
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
        Event.TriggerEvent((int)PetEventName.OnInteractArrive);
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
    public void OnHold()
    {
        Rigid.velocity = Vector3.zero;
        Rigid.isKinematic = true;
        Coll.enabled = false;
        SetNavEnabled(false);
    }

    public void OnDrop()
    {
        Coll.enabled = true;
        Rigid.isKinematic = false;
        Rigid.velocity = Vector3.zero;
        SetNavEnabled(true);
    }

    public void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        Rigid.velocity = Vector3.zero;
        Rigid.isKinematic = false;
        Rigid.AddForce(force, forceMode);
        Event.TriggerEvent((int)PetEventName.OnThrew);
    }

    public void OnThrow()
    {
        Coll.enabled = true;
        Rigid.constraints = RigidbodyConstraints.None;
    }

    public void OnLanding()
    {
        Rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        Rigid.velocity = Vector3.zero;
    }
    #endregion
}