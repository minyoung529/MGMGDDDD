using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    static List<CinemachineVirtualCameraBase> cameras = new List<CinemachineVirtualCameraBase>();

    public static CinemachineVirtualCameraBase activeCamera = null;

    public static bool isAim = false;

    public static bool IsActiveCamera(CinemachineVirtualCameraBase cam)
    {
        return cam == activeCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCameraBase cam, bool rotate)
    {
        isAim = rotate;
        cam.Priority = 10;
        activeCamera= cam;
        foreach(CinemachineVirtualCameraBase c in cameras)
        {
            if (c != cam && c.Priority !=0)
            {
                c.Priority = 0;
            }
        }
    }
    
    
    public static void Register(CinemachineVirtualCameraBase cam)
    {
        cameras.Add(cam);
    }
    
    public static void UnRegister(CinemachineVirtualCameraBase cam)
    {
        cameras.Remove(cam);
    }

}
