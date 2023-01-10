using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using Cinemachine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook firstCamera;
    [SerializeField] CinemachineFreeLook secondCamera;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        CameraSwitcher.Register(firstCamera);
        CameraSwitcher.Register(secondCamera);
        CameraSwitcher.SwitchCamera(firstCamera);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraSwitcher.SwitchCamera(secondCamera);
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraSwitcher.SwitchCamera(firstCamera);
        }
    }
}
