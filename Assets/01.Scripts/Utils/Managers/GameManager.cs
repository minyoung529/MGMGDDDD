using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Camera MainCam { get; private set; }
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

    private float st;

    [SerializeField]
    private LayerMask cameraHitLayerMask;

    #region 퍼즐 관련 변수
    private ButtonObject[] buttons;
    public ButtonObject[] Buttons => buttons;
    private Pet[] pets;
    public Pet[] Pets => pets;
    #endregion

    protected override void Awake()
    {
        FindPuzzleObjects();
        st = Time.time;
        MainCam = Camera.main;
        base.Awake();
    }

    private void Start() {
        // LATER FIX
        SceneController.ListeningEnter(SceneType.Clock, () => MainCam = Camera.main);
        RenderSettingController.Start();
        CameraSwitcher.Start();
    }


    private void FindPuzzleObjects() {
        buttons = FindObjectsOfType<ButtonObject>();
        pets = FindObjectsOfType<Pet>();
    }

    public Vector3 GetMousePos() {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, MainCam.farClipPlane, Define.BOTTOM_LAYER)) {
            Debug.DrawRay(MainCam.transform.position, hit.point);
            Vector3 mouse = hit.point;
            mouse.y = 0;
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

    public T GetNearest<T>(Transform one, T[] targets, float range = float.MaxValue) where T : MonoBehaviour {
        T target = default;
        float min = Mathf.Pow(range, 2);
        foreach (T item in targets) {
            float distance = (transform.position - item.transform.position).sqrMagnitude;
            if (distance < min) {
                min = distance;
                target = item;
            }
        }
        return target;
    }
}