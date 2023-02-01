using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public abstract class Pet : MonoBehaviour
{
    // Test 때문에 Serializefield 해놓은 거임

    #region CheckList
    [SerializeField] protected bool isGet = false;
    [SerializeField] protected bool isMove = false;
    [SerializeField] protected bool isStop = false;
    [SerializeField] protected bool isSkilling = false;
    [SerializeField] protected bool isSelected = false;
    [SerializeField] protected bool isConnected = false;
    [SerializeField] protected bool isActiveCoolTime = false;
    [SerializeField] protected bool isPassiveCoolTime = false;
    public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
    public bool IsGet { get { return isGet; } }
    public bool IsConnected { get { return isConnected; } }
    public bool IsStop { get { return isStop; } }
    public bool IsSkilling { get { return isSkilling; } }
    public bool IsActiveCoolTime { get { return isActiveCoolTime; } }
    public bool IsPassiveCoolTime { get { return isPassiveCoolTime; } }
    #endregion

    [SerializeField] protected float passiveCoolTime = 10.0f;
    [SerializeField] protected float activeCoolTime = 10.0f;

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
        // 1. 얻었냐
        if (!IsGet) return;

        // 2. 연결됐냐
        if (!IsConnected)
        {
            FollowTarget(true);
        }
        else
        {
            // 3. 선택됐냐
            if (!IsSelected) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActiveSkill();
            }
            if (Input.GetMouseButtonDown(1))
            {
                MovePoint();
            }
            ClickMove();
        }

        // active skill 중 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            ClickActive();
        }
    }

    #region SET

    protected virtual void ResetPet()
    {
        isGet = false;
        isMove = false;
        isStop = false;
        isSelected = false;
        isSkilling = false;
        isConnected = false;
        isActiveCoolTime = false;
        isPassiveCoolTime = false;

        ////////////////////////////////// 임시로 FALSE ////////////////////////////////////
        agent.enabled = false;
    }

    public virtual void AppearPet()
    {

    }

    public void Connected()
    {
        isConnected = true;

        agent.enabled = true;
        rigid.useGravity = true;
        rigid.isKinematic = false;
    }

    public void GetPet(GameObject obj)
    {
        player = obj;
        isGet = true;
        
        StartListen();
        Connected();
        FollowTarget(false);
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
    protected void FollowTarget(bool isFollow)
    {
        if (IsStop) return;
        if (isFollow)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
    }

    #endregion

    #region Skill

    protected virtual void ActiveSkill()
    {
        if (!ThirdPersonCameraControll.IsPetAim || !IsSelected || IsActiveCoolTime) return;

        Debug.Log(gameObject.name + " : ActiveSkill Ready");

        isSkilling = true;
        isConnected = false;
        FollowTarget(true);
    }
    protected virtual void ClickActive()
    {
        if (!IsSkilling || !ThirdPersonCameraControll.IsPetAim) return;

        isSelected = false;
        PetManager.Instance.NotSelectPet();
        Debug.Log(gameObject.name + " : ActiveSkill On");
    }

    protected virtual void PassiveSkill(Collision collision)
    {
        if (IsPassiveCoolTime) return;

        Debug.Log(gameObject.name + " : PassiveSkill");
    }
    protected virtual void PassiveSkill()
    {
        if (IsPassiveCoolTime) return;

        Debug.Log(gameObject.name + " : PassiveSkill");
    }

    protected void CoolTime(string str)
    {
        if (str == Define.ACTIVE_COOLTIME_TYPE)
        {
            isActiveCoolTime = true;
            StartCoroutine(StartCool(str, activeCoolTime));
        }
        else if (str == Define.PASSIVE_COOLTIME_TYPE)
        {
            isPassiveCoolTime = true;
            StartCoroutine(StartCool(str, passiveCoolTime));
        }
    }
    private IEnumerator StartCool(string str, float t)
    {
        yield return new WaitForSeconds(t);
        if (str == Define.ACTIVE_COOLTIME_TYPE)
        {
            isActiveCoolTime = false;
        }
        else if (str == Define.PASSIVE_COOLTIME_TYPE)
        {
            isPassiveCoolTime = false;
        }
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
