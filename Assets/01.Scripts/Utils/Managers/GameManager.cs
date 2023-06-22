using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    #region CORE
    public UIManager UI { get; private set; } = new UIManager();
    #endregion

    private PlayerController playerController = null;
    public PlayerController PlayerController
    {
        get
        {
            if (playerController)
                return playerController;

            else
                playerController = FindObjectOfType<PlayerController>();
            return playerController;
        }
    }

    #region 퍼즐 관련 변수
    private ButtonObject[] buttons;
    public ButtonObject[] Buttons => buttons;
    public Pet[] Pets => PetManager.Instance.GetPetList.ToArray();
    //  private IThrowable[] throwables;
    // private IThrowable[] Throwables => throwables;
    #endregion

    private float st;

    [SerializeField]
    private LayerMask cameraHitLayerMask;

    private Vector3 mouseHit;

    protected override void Awake()
    {
        SaveSystem.Load();
        FindPlayer();
        FindFindableObject();
        st = Time.time;

        base.Awake();
    }

    private void Start()
    {
        // LATER FIX
        SceneController.ListeningEnter(SetMainCamera);
        SceneController.ListeningEnter(FindPlayer);
        SceneController.ListeningEnter(FindFindableObject);
        RenderSettingController.Start();
        CameraSwitcher.Start();

        SceneController.ListeningEnter(SceneType.StartScene, CursorEnabled);

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (i != (int)SceneType.StartScene)
            {
                SceneController.ListeningEnter((SceneType)i, CursorDisabled);
            }
        }
    }

    private void FindPlayer()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    [ContextMenu("FindFindableObject")]
    private void FindFindableObject()
    {
        buttons = FindObjectsOfType<ButtonObject>();
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

    public void SetCursorVisible(bool visible)
    {
        Pet.IsCameraAimPoint = !visible;
        Cursor.visible = visible;

        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CursorEnabled() => SetCursorVisible(true);
    public void CursorDisabled() => SetCursorVisible(false);

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
        SceneController.StopListeningEnter(SceneType.StartScene, CursorEnabled);

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (i != (int)SceneType.StartScene)
            {
                SceneController.StopListeningEnter((SceneType)i, CursorDisabled);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save(SaveSystem.CurSaveData);
    }
}