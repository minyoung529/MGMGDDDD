using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    // Test 때문에 Serializefield 해놓은 거임

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
        //InputManager.StartListeningInput(InputAction.Active_Skill, InputType.GetKeyDown, ActiveSkill);
        //InputManager.StartListeningInput(InputAction.Click_Move_Pet, InputType.GetKeyDown, MovePoint);
    }

    private void OnEnable()
    {
        ResetPet();
    }

    protected virtual void Update()
    {
        if (!IsGet) return;
        FollowTarget();

        if (!ThirdPersonCameraControll.IsPetAim) return;
        if (!IsSelected) return;
        if (Input.GetMouseButtonDown(0))
        {
            ActiveSkill();
           
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFollow();
        }

        if (!isSkilling && Input.GetMouseButtonDown(1))
        {
            MovePoint();
        }
        ClickMove();

        // active skill 중 좌클릭 시
        //if (isSkilling && Input.GetMouseButtonDown(0))
        //{
        //    ClickActive();
        //}
    }

    #region SET

    protected virtual void ResetPet()
    {
        isGet = false;
        isMove = false;
        isSelected = false;
        isSkilling = false;
        isCoolTime = false;

        ////////////////////////////////// 임시로 FALSE ////////////////////////////////////
        agent.enabled = false;
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

    // Connected State
    private void SetDestination(Vector3 dest)
    {
        StopFollow();
        destination = dest;
        isMove = true;
        rigid.velocity = Vector3.zero;
    }

    private void MovePoint()
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected) return;

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
    }

    #endregion

    #region Skill

    protected virtual void ActiveSkill()
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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (IsGet) return;
            GetPet(collision.gameObject);
        }
    }

}
