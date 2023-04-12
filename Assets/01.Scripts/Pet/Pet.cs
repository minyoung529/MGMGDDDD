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
    [SerializeField] private ParticleSystem flyParticlePref;
    [SerializeField] private ParticleSystem arriveParticlePref;
    private ParticleSystem flyParticle = null;
    private ParticleSystem arriveParticle = null;

    #region CheckList

    private bool isCoolTime = false;
    protected bool isMouseMove = false;
    private bool isInputLock = false;
    public bool IsInputLock { get { return isInputLock; } set { isInputLock = value; } }
    private bool isRecall = false;
    public bool IsHolding = false;

    #endregion

    protected Collider coll;
    protected Rigidbody rigid;
    protected Transform player;
    protected Transform target;
    protected NavMeshAgent agent;
    protected PetThrow petThrow;
    private float beginAcceleration;
    public float AgentAcceleration
    {
        get => agent.acceleration;
        set { agent.acceleration = value; }
    }

    private Vector3 originScale;

    private ChangePetEmission emission;

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

    private readonly float distanceToPlayer = 5f;

    public Action OnArrive { get; set; }

    private static bool isCameraAimPoint = true;
    public static bool IsCameraAimPoint
    {
        get => isCameraAimPoint;
        set => isCameraAimPoint = value;
    }

    public AxisController AxisController { get; set; }

    protected virtual void Awake()
    {
        originScale = transform.localScale;

        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider>();
        petThrow = GetComponent<PetThrow>();
        emission = GetComponentInChildren<ChangePetEmission>();

        AxisController = new AxisController(transform);
        beginAcceleration = agent.acceleration;

        flyParticle = Instantiate(flyParticlePref, transform);
        arriveParticle = Instantiate(arriveParticlePref, transform);
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

    public virtual void SkillUp()
    {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    #endregion

    #region Move

    private void FollowTarget()
    {
        if (!target || !agent.isOnNavMesh) return;
        agent.SetDestination(target.position);
    }

    public void SetTarget(Transform target, float stopDistance = 0, Action onArrive = null)
    {
        rigid.velocity = Vector3.zero;
        this.target = target;
        agent.stoppingDistance = stopDistance;
        
        if (!target)
        {
            agent.ResetPath();
            return;
        }

        SetNavEnabled(true);
        SetNavIsStopped(false);
        this.OnArrive = onArrive;
    }

    public void SetDestination(Transform target)
    {
        SetTarget(target);
    }

    public void SetTargetPlayer()
    {
        SetNavEnabled(true);
        SetNavIsStopped(false);
        rigid.velocity = Vector3.zero;
        target = player;
        agent.stoppingDistance = distanceToPlayer;
    }

    public void SetPlayerTransform(Transform player)
    {
        this.player = player;
    }

    public void SetDestination(Vector3 target, float stopDistance = 0, Action onArrive = null)
    {
        if (!agent.isOnNavMesh) return;
        this.OnArrive = onArrive;
        SetNavEnabled(true);
        SetNavIsStopped(false);
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

    public void ReCall() {
        Debug.Log(IsHolding);
        if (isRecall || IsHolding || !player) return;
        isRecall = true;
        isInputLock = true;

        isInputLock = true; 
        SetNavEnabled(false);
        coll.enabled = false;
        rigid.isKinematic = true;

        // Default Color: White
        emission.EmissionOn();

        //Darw Bezier
        Vector3 dest = player.position + (transform.position - player.position).normalized * 2f;
        dest = GetNearestNavMeshPosition(dest) + Vector3.up * 1.5f;

        Vector3[] path = new Vector3[3];
        path[0] = dest + Vector3.up;
        path[1] = Vector3.Lerp(transform.position, path[0], 0.2f) + Vector3.up * 5f;
        path[2] = Vector3.Lerp(transform.position, path[0], 0.8f) + Vector3.up * 3f;

        flyParticle.Play();

        transform.DOLookAt(player.position, 0.5f);
        transform.DOPath(path, 3f, PathType.CubicBezier).SetEase(Ease.InSine).OnComplete(() => {
            emission.EmissionOff();
            flyParticle.Stop();
            arriveParticle.Play();
            petThrow.Throw(dest, Vector3.up * 300, 1f);
            isRecall = false;
        });
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
    public Vector3 GetNearestNavMeshPosition(Vector3 position) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 5f, NavMesh.AllAreas)) {
            return hit.position;
        }
        else {
            return position;
        }
    }
    #endregion

    #region InputEvent
    public void MovePoint(bool selected = false)
    {
        if (isInputLock) return;

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

    #region AI
    /// <summary>
    /// 시야 범위에 존재하는 활성화 되지 않은 버튼을 찾아낸 후 타겟으로 설정
    /// </summary>
    /// <returns>탐색 성공 여부</returns>
    public bool FindButton()
    {
        ButtonObject target = GameManager.Instance.GetNearest(transform, GameManager.Instance.Buttons, sightRange);
        if (!target) return false;
        Vector3 dest = target.transform.position;
        agent.SetDestination(dest);
        return true;
    }
    #endregion

    public void ResetAgentValue()
    {
        agent.acceleration = beginAcceleration;
    }
}