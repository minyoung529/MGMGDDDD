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
    public NavMeshAgent Agent => agent;

    // Target
    private float chaseDistance = 15f;

    private Transform target = null;
    public Transform Target => target;

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

        GameManager.Instance.CursorDisabled();
    }

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    public void ChangeState(BossStateName state)
    {
        stateMachine.ChangeState((int)state);
    }

    public void CheckTarget()
    {
        // Ray로 타겟 찾기
        Collider[] coll = Physics.OverlapSphere(transform.position, chaseDistance, (1 << Define.PLAYER_LAYER));
        if(coll.Length > 0)
        {
            target = coll[0].transform;
            ChangeState(BossStateName.Chase);
        }
    }
}
