using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private bool isGet = false;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isConnected = false;
    [SerializeField] private bool isMove = false;

    public bool IsGet() { return isGet; }
    public bool IsSelected() { return isSelected; }
    public bool IsConnected() { return isConnected; }

    private NavMeshAgent agent;
    private Vector3 destination = Vector3.zero;
    protected Camera camera;

    public PetType type;
    public Color selectColor;

    private void Awake()
    {
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
        if (!IsGet()) return;

        // 2. 연결됐냐
        // 3. 선택됐냐
        if (IsConnected())
        {
            if (!IsSelected()) return;
            if (PetManager.instance.IsAltPress())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Skill();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    ClickMove();
                }
            }
            Move();
        }
        else
        {
            FollowTarget(true);
        }
    }

    #region SET

    protected virtual void ResetPet()
    {
        isGet = false;
        isMove = false;
        isSelected = false;
        isConnected = false;
    }

    public void OnConnected(bool isOn)
    {
        isConnected = isOn;
        Follow(!isOn);
    }
    public void OnSelected(bool isOn)
    {
        isSelected = isOn;
    }
    public void OnGetPet(bool isOn)
    {
        isGet = isOn;

        if (isOn)
        {
            OnConnected(true);
            PetManager.instance.AddPet(this);
        }
        else
        {
            PetManager.instance.DeletePet(this);
        }
    }

    #endregion

    #region MOVE

    // Connected State
    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
    }

    private void ClickMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            SetDestination(hit.point);
        }
    }

    private void Move()
    {
        if (isMove)
        {
            if (Vector3.Distance(destination, transform.position) <= 0.1f)
            {
                isMove = false;
                return;
            }
            var dir = destination - transform.position;
            transform.position += dir.normalized * Time.deltaTime * 5f;
        }
    }

    private void FollowTarget(bool isFollow)
    {
        if (isFollow)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.ResetPath();
        }
    }

    // Not Connected State
    private void Follow(bool isFollow)
    {
        if (isFollow == false)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
        else
        {
            agent.isStopped = false;
        }
    }

    #endregion

    protected virtual void Skill()
    {
        OnConnected(false);
        OnSelected(false);
        PetManager.instance.OnSelect(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (IsGet()) return;
            OnGetPet(true);
        }
    }

}
