using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] Texture2D skillCursor;

    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineVirtualCamera ropeAimCamera;
    [SerializeField] CinemachineVirtualCamera petCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ���ӵ�
    [SerializeField] private float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private const float rotationSpeed = 10.0f; // ȸ�� �ӵ�

    private const float limitMinX = -20; // ī�޶� y�� ȸ�� ���� (�ּ�)
    private const float limitMaxX = 20; // ī�޶� y�� ȸ�� ���� (�ִ�)

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��

    private static bool isRopeAim = false;
    private static bool isPetAim = false;

    public static bool IsRopeAim { get { return isRopeAim; } set { isRopeAim = value; } }
    public static bool IsPetAim { get { return isPetAim; } set { isPetAim = value; } }

    private void Start()
    {
        ResetCamera();
    }

    #region Camera Set
    private void ResetCamera()
    {
        CameraSwitcher.UnRegister(defaultCamera);
        CameraSwitcher.UnRegister(ropeAimCamera);
        CameraSwitcher.UnRegister(petCamera);
        CameraSwitcher.Register(defaultCamera);
        CameraSwitcher.Register(ropeAimCamera);
        CameraSwitcher.Register(petCamera);

        SetDefault();
    }
    private void SetDefault()
    {
        isPetAim = false;
        isRopeAim = false;

        MouseCursor.MouserCursorEdit(false, CursorLockMode.Locked);
        CameraSwitcher.SwitchCamera(defaultCamera);
    }
    private void SetPet()
    {
        isPetAim = !isPetAim;
        if (isPetAim)
        {
            MouseCursor.MouserCursorEdit(true, CursorLockMode.None);
            MouseCursor.EditCursorSprite(skillCursor);
            CameraSwitcher.SwitchCamera(petCamera);
        }
        else
        {
            SetDefault();
        }
    }
    private void SetRope()
    {
        isRopeAim = !isRopeAim;
        if (isRopeAim)
        {
            CameraSwitcher.SwitchCamera(ropeAimCamera);
        }
        else
        {
            SetDefault();
        }

    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (isRopeAim) return;
            SetPet();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isPetAim) return;
            SetRope();
        }

        if (isRopeAim)
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
