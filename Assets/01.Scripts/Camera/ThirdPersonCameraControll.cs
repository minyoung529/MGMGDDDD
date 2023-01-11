using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f; // 카메라 y축 회전속도

    private const float rotationSpeed = 10.0f; // 회전 속도

    private const float limitMinX = -20; // 카메라 y축 회전 범위 (최소)
    private const float limitMaxX = 20; // 카메라 y축 회전 범위 (최대)

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우스 위 / 아래 이동으로 카메라 x축 회전

    bool isAim = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        CameraSwitcher.Register(defaultCamera);
        CameraSwitcher.Register(aimCamera);
        CameraSwitcher.SwitchCamera(defaultCamera, false);
    }

    private void SetAim(bool aim)
    {
        isAim = aim;
        if (aim)
        {
            CameraSwitcher.SwitchCamera(aimCamera, true);
        }
        else
        {
            CameraSwitcher.SwitchCamera(defaultCamera, false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetAim(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            SetAim(false);
        }

        if (isAim)
        {
            UpdateRotate();
        }
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        CalculateRotation(mouseX, mouseY);
    }


    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    // 카메라 x축 회전의 경우 회전 범위를 설정
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
