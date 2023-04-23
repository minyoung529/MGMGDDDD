using PathCreation;
using PathCreation.Examples;
using SplineMesh;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRail : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    float timer;
    [SerializeField]
    Spline spline;

    private Transform railPosition;

    private bool isRiding = false;
    public bool IsRiding => isRiding;

    private PathFollower pathFollower;
    private Transform triggerPoint;

    private void Awake()
    {
        railPosition = transform.Find("RailPosition");

        pathFollower ??= Utils.GetOrAddComponent<PathFollower>(gameObject);
        pathFollower.offset = railPosition.localPosition;
        pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;
    }

    public void SetRail(PathCreator path, Transform triggerPoint)
    {
        if (isRiding) return;

        pathFollower.pathCreator = path;
        this.triggerPoint = triggerPoint;
    }

    public void RideRail(InputAction action, float val)
    {
        if (isRiding) return;
        isRiding = true;

        pathFollower.ReasetData();

        if (Vector3.Distance(pathFollower.EndPoint, triggerPoint.position) < 5f)
        {
            pathFollower.reverseStartEnd = true;
            pathFollower.destination = pathFollower.StartPoint;
        }
        else
        {
            pathFollower.reverseStartEnd = false;
            pathFollower.destination = pathFollower.EndPoint;
        }

        pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;

        pathFollower.speed = 15f;
        pathFollower.StartFollowing();

        pathFollower.onArrive.AddListener(OnArrive);
    }

    public void OnArrive(Destination destination)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isRiding = false;
    }

    public void Enter()
    {
        InputManager.StartListeningInput(InputAction.Interaction, RideRail);
    }

    public void Exit()
    {
        triggerPoint = null;
        pathFollower.pathCreator = null;

        InputManager.StopListeningInput(InputAction.Interaction, RideRail);
    }
}
