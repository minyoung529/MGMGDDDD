using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BossState
{
    // Waypoint
    private Transform[] waypoints;
    private Transform curWaypoint;
    private int curWaypointCount = 0;

    public override BossStateName StateName => BossStateName.Patrol;

    public override void OnEnter()
    {
        curWaypointCount = 0;
        
        float minDistance = Vector3.Distance(boss.transform.position, waypoints[curWaypointCount].position);
        for(int i=1; i<waypoints.Length; i++)
        {
            float dis = Vector3.Distance(boss.transform.position, waypoints[i].position);
            if(dis < minDistance)
            {
                minDistance = dis;
                curWaypointCount = i;
            }
        }

        RoamingWaypoint();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    // Patrol State
    private void RoamingWaypoint()
    {
        curWaypointCount++;
        if (curWaypointCount >= waypoints.Length) curWaypointCount = 0;
        boss.SetDestination(waypoints[curWaypointCount].position);
    }
}
