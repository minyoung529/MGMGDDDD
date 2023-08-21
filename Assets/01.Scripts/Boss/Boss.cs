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

    private bool canFind = true;

    [SerializeField]
    private LayerMask targetLayer;

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
        EventManager.StartListening(EventName.BossDetectObject, DetectWaypoint);

        EventManager.StartListening(EventName.InPlayerCupboard, SetFindTargetState);
        EventManager.StartListening(EventName.InPlayerCupboard, ResetTarget);
        EventManager.StartListening(EventName.OutPlayerCupboard, SetFindTargetState);
    }

    private void Update()
    {
        // 레이더로 변경

        //stateMachine.OnUpdate();
        //if (target && canFind)
        //{
        //    float distance = Vector3.Distance(agent.transform.position, target.position);
        //    Debug.Log(target.name);
        //    if (distance <= agent.stoppingDistance)
        //    {
        //        ChangeState(BossStateName.Catch);
        //    }
        //}
    }

    #region Waypoint

    private void DetectWaypoint(EventParam eventParam = null)
    {
        if (eventParam.Contain("DetectPosition"))
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

    private void SetFindTargetState(EventParam eventParam = null)
    {
        if (eventParam.Contain("State"))
        {
            canFind = (bool)eventParam["State"];
            if (!canFind)
            {
                ResetTarget();
            }
            Debug.Log(canFind);
        }
    }
    public void ResetTarget(EventParam eventParam = null)
    {
        target = null;
    }
    public void CheckTarget()
    {
        // Ray로 타겟 찾기
        //Collider[] coll = Physics.OverlapSphere(transform.position, chaseDistance, targetLayer);
        //if (coll.Length > 0)
        //{
        //    if (canFind)
        //    {
        //        target = coll[0].transform;
        //        SetItemWaypoint(coll[0].transform);
        //        ChangeState(BossStateName.Chase);
        //    }
        //}
    }
    private void NotFind()
    {
        ChangeState(BossStateName.Idle);
    }
    #endregion

    #region Event
    public void OnEnterInnerRadar(GameObject obj)
    {
        // 이미 잡는 중이면 Return
        if (stateMachine.CurStateIndex == (int)BossStateName.Catch ||
           stateMachine.CurStateIndex == (int)BossStateName.PetCatch)
            return;

        ChangeState(BossStateName.Catch);
    }

    public void OnExitInnerRadar(GameObject obj)
    {
    }

    public void OnEnterOuterRadar(GameObject obj)
    {
        target = obj.transform;
    }

    public void OnExitOuterRadar(GameObject obj)
    {
        ResetTarget();  // TEMP
    }


    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.BossDetectObject, DetectWaypoint);
        EventManager.StopListening(EventName.InPlayerCupboard, ResetTarget);
        EventManager.StopListening(EventName.InPlayerCupboard, SetFindTargetState);
        EventManager.StopListening(EventName.OutPlayerCupboard, SetFindTargetState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EasterEgg egg = collision.gameObject.GetComponent<EasterEgg>();

        if (egg)
        {
            //ChangeState(BossStateName.stun);
        }
    }
}
public enum BossEventName
{
    OnEnterInnerRadar,
    OnExitInnerRadar,
    OnEnterOuterRadar,
    OnExitOuterRadar,

    DetectObject,
    Count
}
