using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PatrolState : BossState
{
    // Waypoint
    private Transform waypointParent;
    private Transform[] waypoints;

    private int beforeWaypointCount = 0;
    private int curWaypointCount = 1;
    private const int minWayCount = 1;

    private Transform detectWay;

    public override BossStateName StateName => BossStateName.Patrol;

    private void Awake()
    {
        waypointParent = GameObject.FindGameObjectWithTag("BossWaypoint").transform;
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }

    public override void OnEnter()
    {
        boss.Anim.ChangeAnimation(BossAnimType.Walk);

        Vector3 target = boss.transform.position;
        if(boss.ItemWaypoint != null)
        {
            detectWay = boss.ItemWaypoint;
            boss.SetItemWaypoint(null);
            target = detectWay.position;

            Debug.Log(target);
        }
        SetNearWaypoint(target);

        if (detectWay) SetDestination(detectWay.position);
        else SetDestination(waypoints[curWaypointCount].position);
    }

    public override void OnExit()
    {
        detectWay= null;
    }

    public override void OnUpdate()
    {
        if (boss.Agent.path == null) return;
        if (boss.Agent.remainingDistance <= boss.Agent.stoppingDistance) //done with path
        {
            RoamingWaypoint();
        }

        boss.CheckTarget();
    }

    private void SetNearWaypoint(Vector3 point)
    {
        beforeWaypointCount = curWaypointCount;
        curWaypointCount = minWayCount;

        float minDistance = Vector3.Distance(point, waypoints[curWaypointCount].position);
        for (int i = 2; i < waypoints.Length; i++)
        {
            float dis = Vector3.Distance(point, waypoints[i].position);
            if (dis < minDistance)
            {
                minDistance = dis;
                curWaypointCount = i;
            }
        }
    }

    // Patrol State
    private void RoamingWaypoint()
    {
        curWaypointCount++;
        if (curWaypointCount >= waypoints.Length) curWaypointCount = minWayCount;
        SetDestination(waypoints[curWaypointCount].position);
    }

    private void SetDestination(Vector3 point)
    {
        Debug.Log(curWaypointCount);
        boss.Agent.SetDestination(point);
    }

}
