using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHolder : MonoBehaviour
{
    CinemachineVirtualCameraBase camBase;

    void Start()
    {
        camBase = GetComponent<CinemachineVirtualCameraBase>();
        if (camBase)
            CameraSwitcher.Register(camBase);
    }

    void OnDestroy()
    {
        if (camBase)
            CameraSwitcher.UnRegister(camBase);
    }
}
