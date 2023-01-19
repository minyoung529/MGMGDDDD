using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    [SerializeField] CinemachineVirtualCamera petCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ���ӵ�
    [SerializeField] private float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private const float rotationSpeed = 10.0f; // ȸ�� �ӵ�

    private const float limitMinX = -20; // ī�޶� y�� ȸ�� ���� (�ּ�)
    private const float limitMaxX = 20; // ī�޶� y�� ȸ�� ���� (�ִ�)

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��

    private bool isAim = false;
    private static bool isPetAim = false;

    private void Start()
    {
        CameraSwitcher.Register(defaultCamera);
        CameraSwitcher.Register(aimCamera);
        CameraSwitcher.Register(petCamera);
        SetDefaultCamera();
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
            SetDefaultCamera();
        }
    }
    
    private void SetPetCamera(bool aim)
    {
        MouseCursor.MouserCursorEdit(true, CursorLockMode.None);
        isPetAim = aim;
        if (aim)
        {
            CameraSwitcher.SwitchCamera(petCamera, true);
        }
        else
        {
            SetDefaultCamera();
        }
    }
    
    private void SetDefaultCamera()
    {
        isPetAim = isAim = false;

        MouseCursor.MouserCursorEdit(false, CursorLockMode.Locked);
        CameraSwitcher.SwitchCamera(defaultCamera, false);
    }
    public void SetPet(bool isAlt)
    {
        isPetAim = isAlt;
        SetPetCamera(isPetAim);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (isAim) return;
            SetPetCamera(!isPetAim);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isPetAim) return;
            SetAim(!isAim);
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
