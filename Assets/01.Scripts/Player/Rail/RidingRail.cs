using PathCreation;
using PathCreation.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RidingRail : MonoBehaviour
{
    private Transform railPosition;

    private bool isRiding = false;
    public bool IsRiding => isRiding;

    [SerializeField]
    private PathFollower pathFollower;
    public PathFollower PathFollower => pathFollower;
    private Transform triggerPoint;
    private Rigidbody rigid;

    private void Awake()
    {
        pathFollower = Utils.GetOrAddComponent<PathFollower>(gameObject);
        rigid = GetComponent<Rigidbody>();

        railPosition = transform.Find("RailPosition");
        pathFollower.offset = (railPosition) ? railPosition.localPosition : Vector3.zero;

        pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;
    }

    public void SetRail(PathCreator path, Transform triggerPoint)
    {
        if (isRiding) return;

        pathFollower.pathCreator = path;
        this.triggerPoint = triggerPoint;
    }

    public void PlayerRideRail(InputAction action = InputAction.Interaction, float val = 0f)
    {
        if (isRiding) return;

        float triggerToEnd = Vector3.Distance(pathFollower.EndPoint, triggerPoint.position);
        float triggerToStart = Vector3.Distance(pathFollower.StartPoint, triggerPoint.position);

        if (triggerToStart > triggerToEnd)
        {
            Ride(pathFollower.StartPoint, true);
            RidePets(pathFollower.StartPoint, true);
        }
        else
        {
            Ride(pathFollower.EndPoint, false);
            RidePets(pathFollower.EndPoint, false);
        }

        Exit(); 
    }

    public void Ride(Vector3 destination, bool reverse, UnityAction<Destination> onArrive = null)
    {
        if (isRiding) return;
        isRiding = true;

        pathFollower.ReasetData();
        pathFollower.reverseStartEnd = reverse;
        pathFollower.destination = destination;

        pathFollower.endOfPathInstruction = EndOfPathInstruction.Stop;

        pathFollower.speed = 15f;
        pathFollower.StartFollowing();

        pathFollower.onArrive.RemoveListener(OnArrive);
        pathFollower.onArrive.AddListener(OnArrive);

        if (onArrive != null)
        {
            pathFollower.onArrive.RemoveListener(onArrive);
            pathFollower.onArrive.AddListener(onArrive);
        }
    }

    private void RidePets(Vector3 destination, bool reverse)
    {
        StartCoroutine(DelayPets(destination, reverse));
    }

    private IEnumerator DelayPets(Vector3 destination, bool reverse)
    {
        List<Pet> petList = PetManager.Instance.GetPetList;
        petList.ForEach(x => x.Agent.enabled = false);

        for (int i = 0; i < petList.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            RidingRail rail = Utils.GetOrAddComponent<RidingRail>(petList[i]);
            rail.SetRail(pathFollower.pathCreator, triggerPoint);

            Pet pet = petList[i];

            UnityAction<Destination> onArrive = (x) =>
            {
                pet.Agent.enabled = true;
                pet.SetTargetPlayer();
            };

            rail.Ride(destination, reverse, onArrive);

            pathFollower.onArrive.RemoveListener(OnArrive);
            pathFollower.onArrive.AddListener(OnArrive);
        }
    }

    public void OnArrive(Destination destination)
    {
        rigid.velocity = Vector3.zero;
        isRiding = false;
    }

    public void Enter()
    {
        InputManager.StopListeningInput(InputAction.Interaction, PlayerRideRail);
        InputManager.StartListeningInput(InputAction.Interaction, PlayerRideRail);
    }

    public void Exit()
    {
        InputManager.StopListeningInput(InputAction.Interaction, PlayerRideRail);
    }
}
