using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePuzzleController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera topCam;
    [SerializeField] PlayerMove playerMove;

    public void EnterGame()
    {
        if (PetManager.Instance.PetCount < 2) return;
         
        CameraSwitcher.SwitchCamera(topCam);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Pet.IsCameraAimPoint = false;
        playerMove.enabled = false;
    }
    
    public void ExitGame()
    {

    }
}
