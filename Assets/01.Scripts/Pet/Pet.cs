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

    protected Dictionary<Material, Color> materialDictionary = new Dictionary<Material, Color>();
    private readonly int _Emission = Shader.PropertyToID("_Emission");

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
        
        beginAcceleration = agent.acceleration;
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

    private void GetMaterials() {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
            materialDictionary.Add(renderers[i].material, renderers[i].material.GetColor(_Emission));
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
        this.onArrive = onArrive;
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
        this.onArrive = onArrive;
        SetNavEnabled(true);
        SetNavIsStopped(false);
        rigid.velocity = Vector3.zero;
        this.target = null;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(target);
    }

    private void CheckArrive()
    {
        if (Vector3.Distance(agent.destination, transform.position) <= 1f)
        {
            onArrive?.Invoke();
            onArrive = null;
        }
    }

    [ContextMenu("ReCall")]
    public void ReCall() {
        SetNavEnabled(false);
        coll.enabled = false;
        rigid.isKinematic = true;

        foreach (Material item in materialDictionary.Keys) {
            item.SetColor(_Emission, Color.white);
        }

        //Darw Bezier
        Vector3 dest = player.position + (transform.position - player.position).normalized * 2f;
        dest = GetNearestNavMeshPosition(dest) + Vector3.up * 2f;

        Vector3[] path = new Vector3[9];
        path[0] = Vector3.Lerp(transform.position, dest, 0.6f) + Vector3.right * 2f;
        path[1] = Vector3.Lerp(transform.position, path[0], 0.2f) + Vector3.up * 6f;
        path[2] = Vector3.Lerp(transform.position, path[0], 0.8f) + Vector3.up * 3f;
        path[3] = Vector3.Lerp(transform.position, dest, 0.4f) + Vector3.left * 2f;
        path[4] = Vector3.Lerp(path[0], path[3], 0.2f) + Vector3.down * 3f;
        path[5] = Vector3.Lerp(path[0], path[3], 0.8f) + Vector3.down * 3f;
        path[6] = dest;
        path[7] = Vector3.Lerp(path[3], dest, 0.2f) + Vector3.up * 6f;
        path[8] = Vector3.Lerp(path[3], dest, 0.8f) + Vector3.up * 3f;

        transform.DOPath(path, 3f).OnComplete(() => {
            foreach (KeyValuePair<Material, Color> pair in materialDictionary) {
                pair.Key.SetColor(_Emission, pair.Value);
            }
            petThrow.Throw(dest, Vector3.up * 300, 2f);
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
    public bool GetIsOnNavMesh() {
        return agent.isOnNavMesh;
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
    public bool FindButton() {
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
