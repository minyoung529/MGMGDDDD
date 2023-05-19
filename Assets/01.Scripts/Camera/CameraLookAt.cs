using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float duration = 1f;

    [SerializeField] private UnityEvent onLookAt;

    private ThirdPersonCameraControll cameraControll;

    private void Start()
    {
        cameraControll = GameManager.Instance.PlayerController.GetComponent<ThirdPersonCameraControll>();
    }

    [ContextMenu("Look At Target")]
    public void LookAtTarget()
    {
        if (cameraControll == null) return;
        if (lookAtTransform == null) return;

        cameraControll.LookAtTarget(lookAtTransform, duration);
        onLookAt?.Invoke();
    }
}
