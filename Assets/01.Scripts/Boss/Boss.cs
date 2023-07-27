using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 
/// Waypoint를 순서대로 순찰
/// 
/// 추격자와 일정 거리 이하로 가까워지면 Chase 상태로
/// Chase 상태에서 일정거리 이상이 되면 다시 Patrol 상태로 가까운 Waypoint부터 다시 순찰
/// 
/// 소리나 빛이 발생 시 그 물체로 Waypoint 설정
/// 도착 시 추격자를 찾지 못했을 시 주변 Waypoint부터 다시 순찰 
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
