using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float duration = 1f;

    private ThirdPersonCameraControll cameraControll;

    private void Start()
    {
        cameraControll = GameManager.Instance.PlayerController.GetComponent<ThirdPersonCameraControll>();
    }

    [ContextMenu("Look At Target")]
    public void LookAtTarget()
    {
        cameraControll.LookAtTarget(lookAtTransform, duration);
    }
}
