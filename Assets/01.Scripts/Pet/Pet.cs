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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (!IsGet()) return;
        if (IsConnected())
        {
            if (!IsSelected()) return;
            Follow(false);

            if (Input.GetMouseButtonDown(0))
            {
                Skill();
            }
            if (Input.GetMouseButtonDown(1))
            {
                ClickMove();
            }

            Move();
        }
        else
        {
            Follow(true);
            Debug.Log("Follow");
        }
    }

    protected virtual void Skill()
    {
        OnConnected(false);
        PetManager.instance.OnSelect(false);
    }

    #region SET

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
        isConnected = isOn;
        if(isOn==false)
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
        if(!isFollow)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if (IsGet()) return;
            PetManager.instance.AddPet(this);
            OnGetPet(true);
        }
    }

}
