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

    [SerializeField] private float skillCoolTime = 2.0f;
    [SerializeField] private float followDistance = 10.0f;

    #region CheckList

    private bool isFollow = false;
    private bool isCoolTime = false;
    private bool isClickMove = false;

    private int petIndex = -1;

    #endregion

    private Camera camera;
    private Rigidbody rigid;
    private Transform target;
    private NavMeshAgent agent;

    private Vector3 destination = Vector3.zero;

    #region Get

    public int Index { get { return petIndex; } }
    public bool IsGet { get { return petIndex != -1; } }
    public bool IsCoolTime { get { return isCoolTime; } }
    public float Distance { get { return Vector3.Distance(transform.position, target.position); } }
    public bool IsFollowDistance { get { return Vector3.Distance(transform.position, target.position) >= followDistance; }}
    public bool CheckSkillActive {  get { return (!ThirdPersonCameraControll.IsPetAim || !PetManager.Instance.IsPetSelected(Index) || IsCoolTime); } }

    #endregion

    private void Awake()
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
        petIndex = -1;

        isFollow = false;
        isCoolTime = false;

        rigid.velocity = Vector3.zero;
        agent.velocity = Vector3.zero;
    }

    public void GetPet(Transform obj)
    {
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
    public void SetIndex(int index)
    {
        petIndex = index;
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
        Quaternion targetRot = transform.rotation;

        if (ThirdPersonCameraControll.IsPetAim)
        {
            targetRot = Quaternion.LookRotation((GameManager.Instance.GetCameraHit() - transform.position), Vector3.up);
        }
        else
        {
            targetRot = Quaternion.LookRotation((target.position - transform.position), Vector3.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    private void MovePoint(InputAction inputAction, float value)
    {
        if (!ThirdPersonCameraControll.IsPetAim || PetManager.Instance.IsPetSelected(petIndex)) return;

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            SetDestination(hit.point);
        }
    }
    private void SetDestination(Vector3 dest)
    {
        StopFollow();

        destination = dest;
        isClickMove = true;
        rigid.velocity = Vector3.zero;
    }
    private void ClickMove()
    {
        if (Vector3.Distance(destination, transform.position) <= 0.6f)
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
        if (isClickMove) { ClickMove(); }
        if (isFollow && agent.destination != target.position) { agent.SetDestination(target.position); }
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
