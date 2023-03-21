using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Pet : MonoBehaviour
{
    [SerializeField] protected PetTypeSO petInform;
    
    #region CheckList

    private bool isGet = false;
    private bool isFollow = false;
    private bool isCoolTime = false;
    private bool isSelected = false;
    private bool isClickMove = false;
    protected bool isMouseMove = false;
    protected bool isForceBlockMove = false;
    protected bool canMove = false;
    #endregion

    private Camera camera;
    protected Rigidbody rigid;
    private Transform target;
    protected NavMeshAgent agent;

    private Vector3 destination = Vector3.zero;
    private Vector3 originScale;

    #region Get

    public bool IsGet { get { return isGet; } }
    public bool IsCoolTime { get { return isCoolTime; } }
    public bool IsSelected { get { return isSelected; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool IsFollow { get { return isFollow; } set { isFollow = value; } }
    public float Distance { get { return Vector3.Distance(transform.position, target.position); } }

    public bool IsFollowDistance { get { return Vector3.Distance(transform.position, target.position) >= petInform.followDistance; } }
    public bool CheckSkillActive {  get { return (!IsSelected || IsCoolTime); } }
    public Vector3 MouseUpDestination { get; private set; }
    public Vector3 Destination => destination;
    public Rigidbody Rigid => rigid;
    public Sprite petSprite => petInform.petUISprite;

    #endregion

    private float stopDistance;

    protected virtual void Awake()
    {
        isGet = false;
        originScale = transform.localScale;

        camera = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        stopDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        ResetPet();
    }
    private void FixedUpdate()
    {
        if (isForceBlockMove) return;
        if (!IsGet) return;

        FollowTarget();
        //LookAtPlayer();
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    #region Set

    protected virtual void ResetPet()
    {
        isCoolTime = false;
        agent.enabled = true;
        CanMove = true;
        transform.localScale = originScale;

        StartFollow();
    }

    public void GetPet(Transform obj)
    {
        isGet = true;
        target = obj;

        StartFollow();
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

    #endregion

    #region Move
    protected void LookAtPlayer()
    {
        Vector3 dir = target.position;
        dir = GameManager.Instance.GetCameraHit();

        Quaternion targetRot = Quaternion.LookRotation((dir - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    protected void MovePoint(InputAction inputAction, float value)
    {
        if (!IsSelected) return;


        StopFollow();
        isMouseMove = true;
        ClickSetDestination(GameManager.Instance.GetCameraHit());
        transform.DOKill();
    }

    protected void MovePoint(Vector3 destination)
    {
        if (!IsSelected) return;

        ClickSetDestination(destination);
        isMouseMove = false;
    }

    private void ClickSetDestination(Vector3 dest)
    {
        if (!CanMove) return;

        agent.SetDestination(dest);
        destination = dest;
        isFollow = false;
        isClickMove = true;
        rigid.velocity = Vector3.zero;
    }

    protected void StopClickMove() {
        isClickMove = false;
        destination = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }

    private void ClickMove()
    {
        if (isClickMove && Vector3.Distance(destination, transform.position) <= 1f)
        {
            isClickMove = false;
            OnMoveEnd();
            return;
        }

        //var dir = destination - transform.position;
        //dir.y = 0;
        //transform.position += dir.normalized * Time.deltaTime * 5f;
    }

    protected void FollowTarget()
    {
        if (!CanMove) return;

        if(isClickMove)
        {
            ClickMove();
        }
        if (isFollow && agent.destination != target.position)
        {
            OnFollowTarget();
            agent.SetDestination(target.position);
        }
    }

    protected virtual void OnFollowTarget() { }


    public void StartFollow()
    {
        isFollow = true;
        agent.stoppingDistance = stopDistance;
        //agent.isStopped = false;
    }

    protected virtual void SkillUp(InputAction inputAction, float value)
    {
        MouseUpDestination = GameManager.Instance.GetCameraHit();
    }

    protected virtual void OnMoveEnd()
    {
    }

    protected void StopFollow()
    {
        isFollow = false;
        agent.ResetPath();

        agent.stoppingDistance = 0f;
        agent.velocity = Vector3.zero;
    }

    #endregion

    #region Withdraw

    protected virtual void Withdraw(InputAction inputAction, float value)
    {
        ResetPet();
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
