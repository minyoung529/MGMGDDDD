using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 
/// Waypoint�� ������� ����
/// 
/// �߰��ڿ� ���� �Ÿ� ���Ϸ� ��������� Chase ���·�
/// Chase ���¿��� �����Ÿ� �̻��� �Ǹ� �ٽ� Patrol ���·� ����� Waypoint���� �ٽ� ����
/// 
/// �Ҹ��� ���� �߻� �� �� ��ü�� Waypoint ����
/// ���� �� �߰��ڸ� ã�� ������ �� �ֺ� Waypoint���� �ٽ� ���� 
/// 
/// </summary>
public class Boss : MonoBehaviour
{
    // State
    [SerializeField] private Transform stateParent = null;
    private StateMachine<Boss> stateMachine;

    // Component
    private NavMeshAgent agent;

    private Transform target = null;
    public Transform Target => target;

    private Action onArriveAction;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        BossState[] compos = stateParent.GetComponents<BossState>();
        BossState[] states = new BossState[(int)PetStateName.Length];
        foreach (BossState item in compos)
        {
            states[(int)item.StateName] = item;
            states[(int)item.StateName].SetUp(transform);
        }
        stateMachine = new StateMachine<Boss>(this, states);
    }

    private void Update()
    {
        if(agent.velocity == Vector3.zero && stateMachine.CurStateIndex != (int)BossStateName.Idle)
        {
            onArriveAction?.Invoke();
            onArriveAction = null;
        }
    }

    public void ChangeState(BossStateName state)
    {
        stateMachine.ChangeState((int)state);
    }

    public void SetDestination(Vector3 point)
    {
        agent.SetDestination(point);
    }
    public void AddArriveEvent(Action act)
    {
        onArriveAction += act;
    }
}
