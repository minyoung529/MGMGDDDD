using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_Rail : TutorialTrigger
{
    [SerializeField]
    private PathCreator path;

    private PlayerRail rail;

    protected override bool Condition(Transform player)
    {
        rail ??= Utils.GetOrAddComponent<PlayerRail>(player);

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

        rail.Enter();
    }

    protected override void OnExit()
    {
        if (rail == null) return;
        rail.Exit();
    }
}
