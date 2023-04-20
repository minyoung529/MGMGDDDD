using Cinemachine;
using System.Collections;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] Texture2D skillCursor;
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] Canvas crosshairCanvas;

    [SerializeField] private float rotCamXAxisSpeed = 5f;
    [SerializeField] private float rotCamYAxisSpeed = 3f;

    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform followTarget;
    private Animator animator;

    private const float rotationSpeed = 20.0f;

    private const float limitMinX = -80;
    private const float limitMaxX = 80;

    private float eulerAngleX;
    private float eulerAngleY;

    [SerializeField]
    private Renderer playerRenderer;

    private void Start()
    {
        ResetCamera();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, defaultCamera.transform.position);
        float normalized = Mathf.Clamp01(Utils.GetNormalizedRange(1f, 3f, distance));
        playerRenderer.material.SetFloat("_Opacity", normalized);
    }

    #region Camera Set
    private void ResetCamera()
    {
        CameraSwitcher.UnRegister(defaultCamera);
        CameraSwitcher.Register(defaultCamera);
        CameraSwitcher.SetDefaultCamera(defaultCamera);

        SetDefault();
    }
    public void SetDefault()
    {
        MouseCursor.MouseCursorEdit(false, CursorLockMode.Locked);
        CameraSwitcher.SwitchCamera(defaultCamera);
    }
    #endregion

    private void UpdateRotate()
    {
        float yAim = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yAim, 0);
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

    private float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }

    private void ResetPos()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.y = defaultCamera.transform.eulerAngles.y;
        transform.eulerAngles = eulerAngles;
    }

    public void ActiveCrossHair()
    {
        crosshairCanvas.gameObject.SetActive(true);
    }

    public void InactiveCrossHair()
    {
        crosshairCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        CutSceneManager.RemoveStartCutscene(InactiveCrossHair);
        CutSceneManager.RemoveEndCutscene(ActiveCrossHair);
    }
}
