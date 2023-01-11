using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pet : MonoBehaviour
{
    [SerializeField] private bool isSelected = false;
    [SerializeField] private bool isConnected = false;
    [SerializeField] private bool isGet = false;
    [SerializeField] Transform player;

    public bool IsConnected() { return isConnected; }
    public bool IsSelected() { return isSelected; }
    public bool IsGet() { return isGet; }
    private bool isMove;

    private NavMeshAgent agent;
    private Camera camera;
    private Vector3 destination;

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
        }
    }

    protected abstract void Skill();

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
        if(!isOn)
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
            agent.Stop();
            return;
        }
        agent.SetDestination(player.transform.position);
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PetManager.instance.AddPet(this);
            OnGetPet(true);
        }
    }

}
