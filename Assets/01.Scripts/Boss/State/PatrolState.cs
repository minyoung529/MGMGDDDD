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

    public override BossStateName StateName => BossStateName.Patrol;

    private void Awake()
    {
        waypointParent = GameObject.FindGameObjectWithTag("BossWaypoint").transform;
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
    }

    public override void OnEnter()
    {
        SetNearWaypoint();
    }

    public override void OnExit()
    {

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

    private void SetNearWaypoint()
    {
        beforeWaypointCount = curWaypointCount;
        curWaypointCount = minWayCount;

        float minDistance = Vector3.Distance(boss.transform.position, waypoints[curWaypointCount].position);
        for (int i = 2; i < waypoints.Length; i++)
        {
            float dis = Vector3.Distance(boss.transform.position, waypoints[i].position);
            if (dis < minDistance)
            {
                minDistance = dis;
                curWaypointCount = i;
            }
        }

        SetDestination(waypoints[curWaypointCount].position);
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
        boss.Agent.SetDestination(point);
    }

}
