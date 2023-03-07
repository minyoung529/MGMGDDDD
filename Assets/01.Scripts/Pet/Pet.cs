using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public abstract class Pet : MonoBehaviour
{
    public PetType petType = PetType.NONE;
    public Sprite petUI;

    [SerializeField] private float skillCoolTime = 2.0f;
    [SerializeField] private float followDistance = 10.0f;

    #region CheckList

    private bool isGet = false;
    private bool isFollow = false;
    private bool isCoolTime = false;
    private bool isSelected = false;
    private bool isClickMove = false;

    #endregion

    private Camera camera;
    private Rigidbody rigid;
    private Transform target;
    private NavMeshAgent agent;

    private Vector3 destination = Vector3.zero;

    #region Get

    public bool IsGet { get { return isGet; } }
    public bool IsCoolTime { get { return isCoolTime; } }
    public bool IsSelected { get { return isSelected; } }
    public float Distance { get { return Vector3.Distance(transform.position, target.position); } }
    public bool IsFollowDistance { get { return Vector3.Distance(transform.position, target.position) >= followDistance; }}
    public bool CheckSkillActive {  get { return (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsCoolTime); } }

    #endregion

    protected virtual void Awake()
    {
        camera = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (!IsGet) return;
        FollowTarget();
        LookAtPlayer();
    }

    #region Set

    protected virtual void ResetPet()
    {
        isGet = false;
        isFollow = false;
        isCoolTime = false;

        rigid.velocity = Vector3.zero;
        agent.velocity = Vector3.zero;
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
        StartCoroutine(SkillCoolTime(skillCoolTime));
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
        if (ThirdPersonCameraControll.IsPetAim)
        {
            dir = GameManager.Instance.GetCameraHit();
        }

        Quaternion targetRot = Quaternion.LookRotation((dir - transform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    private void MovePoint(InputAction inputAction, float value)
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected) return;

        SetDestination(GameManager.Instance.GetCameraHit());
    }
    private void SetDestination(Vector3 dest)
    {
        StopFollow();

        destination = dest;
        isFollow = false;
        isClickMove = true;
        rigid.velocity = Vector3.zero;
    }
    private void ClickMove()
    {
        if (Vector3.Distance(destination, transform.position) <= 1f)
        {
            isClickMove = false;
            return;
        }
        var dir = destination - transform.position;
        dir.y = 0;
        transform.position += dir.normalized * Time.deltaTime * 5f;
    }

    protected void FollowTarget()
    {
        if (isClickMove)
        {
            ClickMove(); 
        }
        if (isFollow && agent.destination != target.position)
        {
            agent.SetDestination(target.position);
        }
    }

    private void StartFollow(InputAction inputAction, float value)
    {
        isFollow = true;
        agent.isStopped = false;
    }
    private void StartFollow()
    {
        isFollow = true;
        agent.isStopped = false;
    }
    private void StopFollow()
    {
        isFollow = false;
        agent.isStopped = true;
        agent.ResetPath();

        agent.velocity = Vector3.zero;
    }

    #endregion

    #region InputSystem
    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StartListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StartListeningInput(InputAction.Pet_Follow, StartFollow);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Skill);
        InputManager.StopListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StopListeningInput(InputAction.Pet_Follow, StartFollow);
    }
    #endregion
}
