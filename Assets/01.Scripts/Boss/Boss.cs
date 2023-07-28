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
        // Ray�� Ÿ�� ã��
        Collider[] coll = Physics.OverlapSphere(transform.position, chaseDistance, (1 << Define.PLAYER_LAYER));
        if(coll.Length > 0)
        {
            target = coll[0].transform;
            ChangeState(BossStateName.Chase);
        }
    }
}
