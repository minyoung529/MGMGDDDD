using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    // Test ?????? Serializefield ????? ????

    #region CheckList
    [SerializeField] protected bool isGet = false;
    [SerializeField] protected bool isMove = false;
    [SerializeField] protected bool isSkilling = false;
    [SerializeField] protected bool isSelected = false;
    [SerializeField] protected bool isFollowing = true;
    [SerializeField] protected bool isCoolTime = false;
    public bool IsGet { get { return isGet; } }
    public bool IsSkilling { get { return isSkilling; } }
    public bool IsFollowing { get { return isFollowing; } }
    public bool IsCoolTime { get { return isCoolTime; } }
    public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
    #endregion

    [SerializeField] protected float activeCoolTime = 3.0f;
    [SerializeField] protected float followDistance = 10.0f;
    [SerializeField] protected float followSpeed = 7.0f;

    protected Camera camera;
    protected Rigidbody rigid;
    protected GameObject player;
    protected NavMeshAgent agent;

    public PetType type;
    public Color selectColor;
    private Vector3 destination = Vector3.zero;

    protected virtual void Awake()
    {
        camera = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Pet_Skill, ActiveSkill);
        InputManager.StartListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StartListeningInput(InputAction.Pet_Follow, StartFollow);
    }

    private void OnEnable()
    {
        ResetPet();
    }

    protected virtual void Update()
    {
        if (!IsGet) return;
        FollowTarget();
        ClickMove();

        if (!ThirdPersonCameraControll.IsPetAim) return;
        if (!IsSelected) return;
    }

    #region SET

    protected virtual void ResetPet()
    {
        isGet = false;
        isMove = false;
        isSelected = false;
        isSkilling = false;
        isCoolTime = false;

        ////////////////////////////////// ????? FALSE ////////////////////////////////////
        agent.enabled = false;
        agent.speed = followSpeed;
    }

    public virtual void AppearPet()
    {

    }

    public void GetPet(GameObject obj)
    {
        player = obj;
        isGet = true;
        agent.enabled = true;
        rigid.useGravity = true;
        rigid.isKinematic = false;

        StartListen();
        StartFollow();
        PetManager.Instance.AddPet(this);
    }
    public void LosePet()
    {
        isGet = false;
        PetManager.Instance.DeletePet(this);
    }

    #endregion

    #region MOVE

    private IEnumerator CheckFollowDistance()
    {
        while (!isFollowing)
        {
            yield return new WaitForSeconds(0.01f);
            if (FollowDistance())
            {
                StartFollow();
                yield return null;
            }
        }
    }

    private bool FollowDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position) >= followDistance;
    }

    // Connected State
    private void SetDestination(Vector3 dest)
    {
        StopFollow();
        destination = dest;
        isMove = true;
        rigid.velocity = Vector3.zero;
    }


    private void MovePoint(InputAction inputAction, float value)
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected || isSkilling) return;

        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            SetDestination(hit.point);
        }
    }
    private void ClickMove()
    {
        if (!isMove) return;
        if (Vector3.Distance(destination, transform.position) <= 0.6f)
        {
            isMove = false;
            return;
        }
        var dir = destination - transform.position;
        dir.y = 0;
        transform.position += dir.normalized * Time.deltaTime * 5f;
    }

    // Not Connected State
    protected void FollowTarget()
    {
        if (!isFollowing) return;

        agent.SetDestination(player.transform.position);
        LookAtPlayer();
    }

    protected void LookAtPlayer()
    {
        Quaternion targetRot = transform.rotation;

        if (ThirdPersonCameraControll.IsPetAim)
        {
            targetRot = Quaternion.LookRotation((GameManager.Instance.GetCameraHit() - transform.position), Vector3.up);
        }
        else
        {
            targetRot = Quaternion.LookRotation((player.transform.position - transform.position), Vector3.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
    }

    private void StartFollow(InputAction inputAction, float value)
    {
        isFollowing = true;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }
    private void StartFollow()
    {
        isFollowing = true;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }
    private void StopFollow()
    {
        isFollowing = false;
        agent.isStopped = true;
        agent.ResetPath();

        StartCoroutine(CheckFollowDistance());
    }


    #endregion

    #region Skill

    protected virtual void ActiveSkill(InputAction inputAction, float value)
    {
        isSkilling = false;
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsCoolTime) return;
        Debug.Log(gameObject.name + " : ActiveSkill Ready");

        isSkilling = true;
        ClickActive();
    }
    protected virtual void ClickActive()
    {
        if (!IsSkilling || !ThirdPersonCameraControll.IsPetAim) return;
        isSkilling = false;
        SkillDelay();

        // isSelected = false;
        // PetManager.Instance.NotSelectPet();
        Debug.Log(gameObject.name + " : ActiveSkill On");
    }


    protected void SkillDelay()
    {
        isCoolTime = true;
        StartCoroutine(StartCool(activeCoolTime));
    }
    private IEnumerator StartCool(float t)
    {
        yield return new WaitForSeconds(t);
        isCoolTime = false;
    }

    #endregion

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, ActiveSkill);
        InputManager.StopListeningInput(InputAction.Pet_Move, MovePoint);
        InputManager.StopListeningInput(InputAction.Pet_Follow, StartFollow);
    }

}
