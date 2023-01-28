using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public abstract class Pet : MonoBehaviour
{

    [SerializeField] private bool isGet = false;
    [SerializeField] private bool isMove = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isConnected = false;
    [SerializeField] private bool isSkilling = false;
    [SerializeField] private bool isCoolTime = false;
    [SerializeField] private float coolTime = 10.0f;

    protected Camera camera;
    protected Rigidbody rigid;
    protected GameObject player;
    protected NavMeshAgent agent;

    public PetType type;
    public Color selectColor;

    private Vector3 destination = Vector3.zero;

   public bool IsGet {get { return isGet; } set { isGet = value; }}
   public bool IsSelected { get { return isSelected; } set { isSelected = value; }}
   public bool IsConnected { get { return isConnected; } set { isConnected = value; }}
   public bool IsStop { get { return isStop; } set { isStop = value; }}
   public bool IsSkilling { get { return isSkilling; } set { isSkilling = value; }}
   public bool IsCoolTime { get { return isCoolTime; } set { isCoolTime = value; } }

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        camera = Camera.main;
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
        // 3. 선택됐냐
        if (IsConnected)
        {
            if (!IsSelected) return;
            if (ThirdPersonCameraControll.IsPetAim)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ActiveSkill();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    MovePoint();
                }
            }
            ClickMove();
        }
        else
        {
            if (IsStop) return;
            FollowTarget(true);
        }

        // active skill 중 좌클릭 시
        if (isSkilling)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickActive();
            }
        }
    }

    #region SET

    protected virtual void ResetPet()
    {
        IsGet = false;
        isMove = false;
        IsStop = false;
        IsSelected = false;
        IsSkilling = false;
        IsConnected = false;
        IsCoolTime = false;

        ////////////////////////////////// 임시로 FALSE ////////////////////////////////////
        agent.enabled = false;
    }

    public void GetPet(GameObject obj)
    {
        player = obj;
        isGet = true;
        IsConnected=true;

        FollowTarget(false);
        PetManager.instance.AddPet(this);
    }
    public void LosePet()
    {
        isGet = false;

        PetManager.instance.DeletePet(this);
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
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            SetDestination(hit.point);
        }
    }
    private void ClickMove()
    {
        if (isMove)
        {
            if (Vector3.Distance(destination, transform.position) <= 0.1f)
            {
                isMove = false;
                return;
            }
            var dir = destination - transform.position;
            rigid.position += dir.normalized * Time.deltaTime * 5f;
        }
    }

    // Not Connected State
    protected void FollowTarget(bool isFollow)
    {
        if (isFollow)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    #endregion

    #region Skill
    
    protected virtual void ActiveSkill()
    {
        Debug.Log(gameObject.name + " : ActiveSkill Ready");

        IsSkilling = true;
        IsConnected = false;
        IsSelected = false;
        FollowTarget(true);
        PetManager.instance.NotSelectPet();
    }
    protected virtual void ClickActive()
    {
        Debug.Log(gameObject.name + " : ActiveSkill On");
    }
    protected virtual void PassiveSkill(Collision collision)
    {
        if (IsCoolTime)
        {
            Debug.Log(gameObject.name + " : Nope Passive CoolTime");
            return;
        }
        Debug.Log(gameObject.name + " : PassiveSkill");
    }
    protected virtual void PassiveSkill()
    {
        if (IsCoolTime)
        {
            Debug.Log(gameObject.name + " : Nope Passive CoolTime");
            return;
        }
        Debug.Log(gameObject.name + " : PassiveSkill");
        CoolTime();
    }

    protected void CoolTime()
    {
        IsCoolTime = true;
        StartCoroutine(StartCool());
    }
    private IEnumerator StartCool()
    {
        yield return new WaitForSeconds(coolTime);
        IsCoolTime = false;
    }

    #endregion

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (IsGet) return;
            //GetPet(collision.gameObject);
        }
    }

}
