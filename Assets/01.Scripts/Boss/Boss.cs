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
    private PlayBossAnimation anim;
    public PlayBossAnimation Anim => anim;

    // Target
    private float chaseDistance = 15f;

    private Transform target = null;
    public Transform Target => target;

    // Event
    private LocalEvent bossEvent = new LocalEvent();
    public LocalEvent Event => bossEvent;

    // Waypoint
    private Transform itemWaypoint;
    public Transform ItemWaypoint => itemWaypoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<PlayBossAnimation>();

        BossState[] compos = stateParent.GetComponents<BossState>();
        BossState[] states = new BossState[(int)BossStateName.Count];
        foreach (BossState item in compos)
        {
            states[(int)item.StateName] = item;
            states[(int)item.StateName].SetUp(transform);
        }
        stateMachine = new StateMachine<Boss>(this, states);

        GameManager.Instance.CursorDisabled();
    }

    private void Start()
    {
        EventManager.StartListening((int)BossEventName.DetectObject, DetectWaypoint);
    }

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    #region Waypoint

    private void DetectWaypoint(EventParam eventParam = null)
    {
        if(eventParam.Contain("DetectPosition"))
        {
            DetectWaypoint((Transform)eventParam["DetectPosition"]);
        }
    }
    public void DetectWaypoint(Transform point)
    {
        itemWaypoint = point;
        ChangeState(BossStateName.Patrol);
    }

    #endregion

    #region State
    public void ChangeState(BossStateName state)
    {
        stateMachine.ChangeState((int)state);
    }
    #endregion

    #region Target
    public void SetItemWaypoint(Transform target)
    {
        itemWaypoint = target;
    }
    public void CheckTarget()
    {
        // Ray�� Ÿ�� ã��
        Collider[] coll = Physics.OverlapSphere(transform.position, chaseDistance, (1 << Define.PLAYER_LAYER));
        if(coll.Length > 0)
        {
            SetItemWaypoint(coll[0].transform);
            ChangeState(BossStateName.Chase);
        }
    }
    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening((int)BossEventName.DetectObject, DetectWaypoint);
    }
}
public enum BossEventName
{
    DetectObject,
    Count
}
