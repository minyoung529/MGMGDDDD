using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] Texture2D skillCursor;
    [SerializeField] Material playerMaterial;

    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] CinemachineFreeLook petAimCamera;

    [SerializeField] private float rotCamXAxisSpeed = 5f; // ???? x?? ??????
    [SerializeField] private float rotCamYAxisSpeed = 3f; // ???? y?? ??????

    [SerializeField] private Canvas crosshair;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform followTarget;
    private Animator animator;

    private const float rotationSpeed = 20.0f; // ??? ???

    private const float limitMinX = -80; // ???? y?? ??? ???? (???)
    private const float limitMaxX = 80; // ???? y?? ??? ???? (???)

    private float eulerAngleX; // ????J ?? / ?? ??????? ???? y?? ???
    private float eulerAngleY; // ????J ?? / ??? ??????? ???? x?? ???

    private static bool isPetAim = false;
    public static bool IsPetAim { get { return isPetAim; } set { isPetAim = value; } }

    private void Start()
    {
        ResetCamera();

        animator = GetComponent<Animator>();
        crosshair = Instantiate(crosshair);
        crosshair.gameObject.SetActive(false);

        CutSceneManager.AddStartCutscene(InactiveCrossHair);
        CutSceneManager.AddEndCutscene(ActiveCrossHair);

        InputManager.StartListeningInput(InputAction.Zoom, SetPet);
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
    
    public void SetPet(InputAction input, float value)
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

    private void FixedUpdate()
    {
        if (IsPetAim) UpdateRotate();
    }

    private void UpdateRotate()
    {
        float yAim = Camera.main.transform.rotation.eulerAngles.y;
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yAim, 0), rotationSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, yAim, 0);
        // float mouseX = Input.GetAxis("Mouse X");
        // float mouseY = Input.GetAxis("Mouse Y");

        //CalculateRotation(mouseX, mouseY);
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
    // ???? x?? ????? ??? ??? ?????? ????
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

    private void ActiveCrossHair()
    {
        if (isPetAim)
        {
            crosshair.gameObject.SetActive(true);
        }
    }

    private void InactiveCrossHair()
    {
        crosshair.gameObject.SetActive(false);
    }

    private void CheckNearPlayer()
    {
        playerMaterial.color = new Color(playerMaterial.color.r, playerMaterial.color.b, playerMaterial.color.b, 0);
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Zoom, SetPet);

        CutSceneManager.RemoveStartCutscene(InactiveCrossHair);
        CutSceneManager.RemoveEndCutscene(ActiveCrossHair);
    }
}
