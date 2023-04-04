using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Camera mainCam = null;
    public Camera MainCam
    {
        get
        {
            if (mainCam == null)
                mainCam = Camera.main;
            return mainCam;
        }
    }

    private CutSceneManager cutSceneManager;
    public CutSceneManager CutSceneManager
    {
        get
        {
            if (cutSceneManager)
                return cutSceneManager;

            cutSceneManager = FindObjectOfType<CutSceneManager>();
            return cutSceneManager;
        }
    }

    #region CORE
    public UIManager UI { get; private set; } = new UIManager();
    #endregion

    #region 퍼즐 관련 변수
    private ButtonObject[] buttons;
    public ButtonObject[] Buttons => buttons;
    private Pet[] pets;
    public Pet[] Pets => pets;
    #endregion

    private float st;

    [SerializeField]
    private LayerMask cameraHitLayerMask;

    private Vector3 mouseHit;

    protected override void Awake()
    {
        FindFindableObject();
        st = Time.time;

        base.Awake();
    }

    private void Start()
    {
        // LATER FIX
        SceneController.ListeningEnter(SetMainCamera);
        RenderSettingController.Start();
        CameraSwitcher.Start();
    }

    private void FindFindableObject()
    {
        buttons = FindObjectsOfType<ButtonObject>();
        pets = FindObjectsOfType<Pet>();
    }

    private void SetMainCamera()
    {
        mainCam = Camera.main;
    }

    public Vector3 GetMousePos()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, MainCam.farClipPlane, cameraHitLayerMask))
        {
            Debug.DrawRay(MainCam.transform.position, ray.direction * hit.distance, Color.cyan);
            Vector3 mouse = hit.point;
            //mouse.y = 0;

            mouseHit = hit.point;
            return mouse;
        }
        return Vector3.zero;
    }

    public Vector3 GetCameraHit()
    {
        Ray ray = MainCam.ViewportPointToRay(Vector2.one * 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MainCam.farClipPlane, cameraHitLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            return hit.point;
        }

        return Vector3.zero;
    }
    public T GetNearest<T>(Transform one, T[] targets, float range = float.MaxValue) where T : MonoBehaviour, IFindable
    {
        T target = default;
        float min = Mathf.Pow(range, 2);
        foreach (T item in targets)
        {
            if (!item.IsFindable) continue;
            float distance = (transform.position - item.transform.position).sqrMagnitude;
            if (distance < min)
            {
                min = distance;
                target = item;
            }
        }
        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mouseHit, 0.2f);
    }

    private void OnDestroy()
    {
        SceneController.StopListeningEnter(SetMainCamera);
    }
}