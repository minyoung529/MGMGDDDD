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

    private Transform playerTransform;
    private Animator animator;
    private Rigidbody playerRigid;

    private Vector3 originalPlayerScale;
    private bool isPalyerArea = false;
    private bool isBoarding = false;

    [SerializeField]
    private UnityEvent OnBoarding;

    [SerializeField]
    private UnityEvent OnQuit;

    private void Awake()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Interaction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            playerTransform = other.transform;
            animator ??= playerTransform.GetComponent<Animator>();
            playerRigid = other.attachedRigidbody;

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

    private void Update()
    {
        if (isBoarding && playerTransform)
        {
            playerTransform.position = playerPos.position;
        }
    }

    private void Interaction(InputAction action, float value)
    {
        if (!isPalyerArea || !playerTransform) return;

        if (isBoarding) // 타고 있다면 하차
        {
            Quit();
            OnQuit?.Invoke();
        }
        else
        {
            Boarding();
            OnBoarding?.Invoke();
        }

        isBoarding = !isBoarding;
    }

    private void Boarding()
    {
        playerTransform.position = playerPos.position;
        playerTransform.SetParent(transform.parent);
        animator.SetInteger("iStateNum", (int)StateName.Sit);
        playerRigid.isKinematic = true;

    }

    private void Quit()
    {
        playerTransform.SetParent(null);
        MaintainPlayerScale();
        animator.SetInteger("iStateNum", (int)0);

        if (playerRigid)
            playerRigid.isKinematic = false;
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
