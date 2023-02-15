using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Camera MainCam { get; private set; }

    #region CORE
    public UIManager UI { get; private set; } = new UIManager();
    #endregion

    private void Awake()
    {
        MainCam = Camera.main;
    }

    private void Start()
    {
        RenderSettingController.Start();
    }

    public Vector3 GetMousePos()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, MainCam.farClipPlane, Define.BOTTOM_LAYER))
        {
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

        if (Physics.Raycast(ray, out hit, MainCam.farClipPlane))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            return hit.point;
        }

        return Vector3.zero;
    }
}