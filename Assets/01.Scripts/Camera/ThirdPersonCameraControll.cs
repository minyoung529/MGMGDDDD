using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] Texture2D skillCursor;

    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineVirtualCamera petAimCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f; // 카메라 y축 회전속도

    [SerializeField] private Canvas crosshair;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform followTarget;
    private Animator animator;

    private const float rotationSpeed = 10.0f; // 회전 속도

    private const float limitMinX = -80; // 카메라 y축 회전 범위 (최소)
    private const float limitMaxX = 80; // 카메라 y축 회전 범위 (최대)

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우스 위 / 아래 이동으로 카메라 x축 회전

    private static bool isPetAim = false;
    public static bool IsPetAim { get { return isPetAim; } set { isPetAim = value; } }

    private void Start()
    {
        ResetCamera();

        animator = GetComponent<Animator>();
        crosshair = Instantiate(crosshair);
        crosshair.gameObject.SetActive(false);
    }

    #region Camera Set
    private void ResetCamera()
    {
        CameraSwitcher.UnRegister(defaultCamera);
        CameraSwitcher.UnRegister(petAimCamera);
        CameraSwitcher.Register(defaultCamera);
        CameraSwitcher.Register(petAimCamera);

        SetDefault();
    }
    private void SetDefault()
    {
        MouseCursor.MouseCursorEdit(false, CursorLockMode.Locked);
        CameraSwitcher.SwitchCamera(defaultCamera);
    }
    public void SetPet()
    {
        isPetAim = !isPetAim;

        if (isPetAim)
        {
            CameraSwitcher.SwitchCamera(petAimCamera);
            eulerAngleX = transform.eulerAngles.x;
            eulerAngleY = transform.eulerAngles.y;

            transform.forward = (transform.position - defaultCamera.transform.position).normalized;
            transform.eulerAngles = transform.eulerAngles.MultiplyVec(new Vector3(0f, 1f, 1f));
        }
        else
        {
            SetDefault();
            SetResetPos();
        }

        crosshair.gameObject.SetActive(isPetAim);
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetPet();
        }

        if(IsPetAim) UpdateRotate();

    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        CalculateRotation(mouseX, mouseY);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1f);
        animator.SetLookAtPosition(lookTarget.position);
    }
    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(transform.rotation.x, eulerAngleY, transform.rotation.z);
        followTarget.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, transform.rotation.z);
    }
    // 카메라 x축 회전의 경우 회전 범위를 설정
    private float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }

    private void SetResetPos()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.y = defaultCamera.transform.eulerAngles.y;
        transform.eulerAngles = eulerAngles;
    }
}
