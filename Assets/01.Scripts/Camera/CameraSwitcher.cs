using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraType
{
    Default,
    Rope,
    Pet
}
public class CameraSwitcher
{
    static List<CinemachineVirtualCameraBase> cameras = new List<CinemachineVirtualCameraBase>();
    public static List<CinemachineVirtualCameraBase> Cameras => cameras;

    public static CinemachineVirtualCameraBase activeCamera = null;

    private static CinemachineBrain cinemachineBrain;
    public static CinemachineBrain CinemachineBrain
    {
        get
        {
            if (cinemachineBrain)
                return cinemachineBrain;

            cinemachineBrain = Object.FindObjectOfType<CinemachineBrain>();
            return cinemachineBrain;
        }
    }


    public static bool IsActiveCamera(CinemachineVirtualCameraBase cam) { return cam == activeCamera; }

    public static void SwitchCamera(CinemachineVirtualCameraBase cam)
    {
        //ChangeSwitchBlend(0.5f);

        cam.Priority = 10;
        activeCamera = cam;
        foreach (CinemachineVirtualCameraBase c in cameras)
        {
            if (c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static void Start()
    {
        SceneController.ListeningEnter(SceneType.Clock, ResetCameras);
    }

    private static void ResetCameras()
    {
        cameras.Clear();
        activeCamera = null;
    }

    #region CameraSet

    public static void Register(CinemachineVirtualCameraBase cam)
    {
        cameras.Add(cam);
    }
    public static void UnRegister(CinemachineVirtualCameraBase cam)
    {
        cameras.Remove(cam);
    }

    #endregion

    public static void ChangeSwitchBlend(float value)
    {
        CinemachineBrain.m_DefaultBlend.m_Time = value;
    }
}
