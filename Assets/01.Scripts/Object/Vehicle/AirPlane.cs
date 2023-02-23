using DG.Tweening;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AirPlane : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private Transform exitPos;

    private Transform playerTransform;
    private Rigidbody playerRigid;

    private bool isPalyerArea = false;
    private bool isBoarding = false;

    [SerializeField]
    private UnityEvent OnBoarding;

    [SerializeField]
    private UnityEvent OnQuit;

    private JumpMotion jumpMotion = new JumpMotion();

    private bool isArrive = false;

    private void Awake()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Interaction);

        OnQuit.AddListener(ResetPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            playerTransform = other.transform;
            playerRigid = other.attachedRigidbody;

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

        if (isBoarding) // 타고 있다면 하차
        {
            if (isArrive)
                QuitAnimation();
        }
        else
        {
            isArrive = false;
            Boarding();
        }

        isBoarding = !isBoarding;
    }

    private void Boarding()
    {
        playerTransform.SetParent(transform.parent);
        playerRigid.isKinematic = true;

        BoardingAnimation();    // 포물선
    }

    private void BoardingAnimation()
    {
        jumpMotion.targetPos = playerPos.position;
        jumpMotion.StartJump(playerTransform, null, () => OnBoarding?.Invoke(), true);
    }


    private void QuitAnimation()
    {
        jumpMotion.targetPos = exitPos.position;
        jumpMotion.StartJump(playerTransform, null, () => OnQuit?.Invoke());
    }

    private void ResetPlayer()
    {
        playerTransform.SetParent(null);

        if (playerRigid)
            playerRigid.isKinematic = false;
    }

    public void Arrive()
    {
        isArrive = true;
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Interaction, Interaction);
    }
}
