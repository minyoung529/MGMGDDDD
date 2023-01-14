using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isConnected = false;
    [SerializeField] private bool isGet = false;
    [SerializeField] private Transform player;

    public bool IsConnected() { return isConnected; }
    public bool IsSelected() { return isSelected; }
    public bool IsGet() { return isGet; }

    private Vector3 destination;
    private NavMeshAgent agent;
    private Camera camera;
    private bool isMove;

    public PetType type;
    public Color selectColor;

    private bool altPress = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        ResetPet();
    }

    private void Update()
    {
        // 1. 얻었냐
        if (!IsGet()) return;

        // 2. 연결됐냐
        // 3. 선택됐냐
        if (IsConnected())
        {
            if (!IsSelected()) return;
            if (Input.GetKeyDown(KeyCode.LeftAlt)) altPress = !altPress;

            Follow(false);
            if (altPress)
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
            Follow(true);
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
    }

    public void OnSelected(bool isOn)
    {
        isSelected = isOn;
    }

    public void OnGetPet(bool isOn)
    {
        isGet = isOn;
        OnConnected(true);

        if (isOn)
        {
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

    // Not Connected State
    private void Follow(bool isFollow)
    {
        if (!isFollow)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    #endregion

    protected virtual void Skill()
    {
        OnConnected(false);
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
