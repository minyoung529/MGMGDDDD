using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ���ӵ�
    [SerializeField] private float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private const float rotationSpeed = 10.0f; // ȸ�� �ӵ�

    private const float limitMinX = -20; // ī�޶� y�� ȸ�� ���� (�ּ�)
    private const float limitMaxX = 20; // ī�޶� y�� ȸ�� ���� (�ִ�)

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��

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

    // ī�޶� x�� ȸ���� ��� ȸ�� ������ ����
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
