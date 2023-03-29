using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSoloCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase changedCamera;

    [ContextMenu("Change Camera")]
    public void Change()
    {
        CameraSwitcher.SwitchCamera(changedCamera);
    }
}
