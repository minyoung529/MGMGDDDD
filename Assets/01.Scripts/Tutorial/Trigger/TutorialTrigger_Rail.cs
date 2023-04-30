using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_Rail : TutorialTrigger
{
    [SerializeField]
    private PathCreator path;

    private RidingRail rail;

    [SerializeField]
    private bool isRotate = true;

    protected override bool Condition(Transform player)
    {
        rail ??= Utils.GetOrAddComponent<RidingRail>(player);

        if (!rail) return false;

        if (!rail.IsRiding)
        {
            rail.SetRail(path, transform);
        }

        return !rail.IsRiding;
    }

    protected override void OnEnter()
    {
        if (rail == null) return;

        rail.PathFollower.IsRotate = isRotate;
        rail.Enter();
    }

    protected override void OnExit()
    {
        if (rail == null) return;

        rail.PathFollower.IsRotate = isRotate;
        rail.Exit();
    }
}
