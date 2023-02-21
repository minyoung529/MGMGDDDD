using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Destination : int
{
    Clock, TV, Count
}

public class Train : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private PathFollower pathFollower;

    [SerializeField]
    private Transform[] destinations;

    private Transform playerTransform;
    private Vector3 originalPlayerScale;
    private bool isPalyerArea = false;
    private bool isBoarding = false;

    private void Awake()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Interaction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            playerTransform = other.transform;
            originalPlayerScale = playerTransform.localScale;
            isPalyerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            playerTransform = null;
            originalPlayerScale = Vector3.zero;
            isPalyerArea = false;
        }
    }

    private void Interaction(InputAction action, float value)
    {
        if (!isPalyerArea || !playerTransform) return;

        if (isBoarding) // 타고 있다면 하차
        {
            Quit();
        }
        else
        {
            Boarding();
        }

        isBoarding = !isBoarding;
    }

    private void Boarding()
    {
        playerTransform.position = playerPos.position;
        pathFollower.destination = destinations[(int)Destination.Clock];

        playerTransform.SetParent(transform);

        pathFollower.Depart();
    }

    private void Quit()
    {
        playerTransform.SetParent(null);
        MaintainPlayerScale();
    }

    private void MaintainPlayerScale()
    {
        playerTransform.localScale = originalPlayerScale;
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Interaction, Interaction);
    }
}
