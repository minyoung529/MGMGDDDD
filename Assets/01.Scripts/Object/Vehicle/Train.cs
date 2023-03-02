using DG.Tweening;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination : int
{
    Table, Clock, TV, Count
}

public class Train : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private Transform exitPos;

    private JumpMotion jumpMotion = new JumpMotion();

    [SerializeField]
    private PathFollower pathFollower;

    [SerializeField]
    private Transform[] destinations;

    private Transform playerTransform;
    private Rigidbody playerRigid;
    private bool isPalyerArea = false;
    private bool isBoarding = false;
    private bool isArrive = false;

    private void Awake()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Interaction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            playerRigid ??= other.attachedRigidbody;
            playerTransform = other.transform;
            isPalyerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            isPalyerArea = false;
        }
    }

    private void Interaction(InputAction action, float value)
    {
        if (!isPalyerArea || !playerTransform) return;

        if (isBoarding && isArrive) // 타고 있다면 하차
        {
            QuitAnimation();
            isBoarding = !isBoarding;
        }
        else if(!isBoarding)
        {
            BoardingAnimation();
            isArrive = false;
            isBoarding = !isBoarding;
        }
    }

    private void Boarding()
    {
        playerRigid.isKinematic = true;
        playerTransform.SetParent(transform.parent);

        pathFollower.destination = destinations[(int)Destination.Clock];
        pathFollower.Depart();
        pathFollower.onArrive.AddListener((x) => isArrive = true);
    }

    private void BoardingAnimation()
    {
        playerTransform.DORotateQuaternion(Quaternion.LookRotation(transform.forward, Vector3.up), 1f);

        jumpMotion.targetPos = playerPos.position;
        jumpMotion.StartJump(playerTransform, null, Boarding);
    }

    private void Quit()
    {
        playerTransform.SetParent(null);
        playerRigid.isKinematic = false;
    }

    private void QuitAnimation()
    {
        jumpMotion.targetPos = exitPos.position;

        jumpMotion.StartJump(playerTransform, null, Quit);
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Interaction, Interaction);
    }
}
