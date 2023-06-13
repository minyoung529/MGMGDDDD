using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ThirdPersonCameraControll : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook defaultCamera;
    [SerializeField] Canvas crosshairCanvas;

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

        CutSceneManager.Instance.AddStartCutscene(InactiveCrossHair);
        CutSceneManager.Instance.AddEndCutscene(ActiveCrossHair);
    }

    private void Update()
    {
        defaultCamera.m_XAxis.m_MaxSpeed = SaveSystem.CurSaveData.hSensitivity * 100f;
        defaultCamera.m_YAxis.m_MaxSpeed = SaveSystem.CurSaveData.vSensitivity;

    //    Debug.Log(SaveSystem.CurSaveData.hSensitivity * 100f + ", " + SaveSystem.CurSaveData.vSensitivity);
        
        float distance = Vector3.Distance(transform.position, defaultCamera.transform.position);
        float normalized = Mathf.Clamp01(Utils.GetNormalizedRange(1.5f, 3f, distance));
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

    #region Look At

    private float CalculateXAxis(Vector3 dir)
    {
        Vector3 yZeroDir = dir.MultiplyVec(new Vector3(1f, 0f, 1f));
        float xAxis = Vector3.Angle(yZeroDir, Vector3.forward);
        Vector3 cross = Vector3.Cross(yZeroDir, Vector3.forward);

        if (cross.y > 0f)
            xAxis *= -1f;

        return xAxis;
    }

    private float CalculateYAxis(Vector3 dir)
    {
        Vector3 xZeroDir = dir.MultiplyVec(new Vector3(0f, 1f, 1f));
        float yAxis = Vector3.Angle(xZeroDir, Vector3.down);

        return 1f - yAxis / 180f;
    }

    public void LookAtTarget(Transform target, float duration = 1f)
    {
        Vector3 dir = target.position - transform.position;

        defaultCamera.m_XAxis.m_InputAxisName = "";
        defaultCamera.m_YAxis.m_InputAxisName = "";

        DOTween.To(() => defaultCamera.m_XAxis.Value, (x) => defaultCamera.m_XAxis.Value = x, CalculateXAxis(dir), duration);
        DOTween.To(() => defaultCamera.m_YAxis.Value, (x) => defaultCamera.m_YAxis.Value = x, CalculateYAxis(dir), duration).OnComplete(() =>
        {
            defaultCamera.m_XAxis.m_InputAxisName = "Mouse X";
            defaultCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        });
    }
    #endregion

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(1f);
        animator.SetLookAtPosition(lookTarget.position);
    }

    #region Crosshair
    public void ActiveCrossHair()
    {
        crosshairCanvas.gameObject.SetActive(true);
    }

    public void InactiveCrossHair()
    {
        crosshairCanvas.gameObject.SetActive(false);
    }
    #endregion

    private void OnDestroy()
    {
        CutSceneManager.Instance?.RemoveStartCutscene(InactiveCrossHair);
        CutSceneManager.Instance?.RemoveEndCutscene(ActiveCrossHair);
    }
}
