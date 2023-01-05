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

    public Vector3 GetMousePos()
    {
        Ray ray = MainCam.ScreenPointToRay(UnityEngine.Input.mousePosition);
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
}