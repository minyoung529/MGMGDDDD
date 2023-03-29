using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazePuzzleController : MonoSingleton<MazePuzzleController>
{
    [SerializeField] CinemachineFreeLook defaultCam;
    [SerializeField] CinemachineVirtualCamera fireCam;
    [SerializeField] CinemachineVirtualCamera oilCam;

    [SerializeField] GameObject exitTrigger;

    [SerializeField] PlayerMove playerMove;
    [SerializeField] Canvas mazeCanvas;
    [SerializeField] Text countText;

    Stack<MazeButton> buttons = new Stack<MazeButton>();

    private const int maxUndoCount = 2;
    private int undoCount = 2;
    private int exitPetCount = 0;

    private void Start()
    {
        RegisterCamera();
        ResetController();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Down_Pet, SwitchPet);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StopListeningInput(InputAction.Down_Pet, SwitchPet);
    }

    private void SwitchPet(InputAction input, float action)
    {
        if(PetManager.Instance.GetSelectPet.GetPetType == PetType.FIRE)
        {
            CameraSwitcher.SwitchCamera(fireCam);
        }
        else if(PetManager.Instance.GetSelectPet.GetPetType == PetType.OIL)
        {
            CameraSwitcher.SwitchCamera(oilCam);
        }
    }

    private void RegisterCamera()
    {
        CameraSwitcher.Register(oilCam);
        CameraSwitcher.Register(fireCam);
    }

    private void ResetController()
    {
        mazeCanvas.gameObject.SetActive(false);
        undoCount = maxUndoCount;
        exitPetCount = 0;
        countText.text = string.Format($"{undoCount}");
    }

    #region EnterGame

    public void EnterGame()
    {
        if (PetManager.Instance.PetCount < 2) return;

        StartListen();
        ChangeCam();
        ThirdPersonCameraControll.OnCrossHairMove(true);
        mazeCanvas.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Pet.IsCameraAimPoint = false;
        playerMove.enabled = false;
    }

    #endregion

    #region Do

    public void DoButton(MazeButton button)
    {
        buttons.Push(button);
    }

    #endregion

    #region Undo

    public void UndoButton()
    {
        if (buttons.Count < 1 || undoCount < 1) return;
        UndoCount();

        MazeButton b = buttons.Peek();
        StartCoroutine(DelayUndo(b));
        buttons.Pop();
        b.Undo();
    }

    private IEnumerator DelayUndo(MazeButton b)
    {
        yield return new WaitForSeconds(0.2f);
        b.ButtonAction();
    }

    private void UndoCount()
    {
        undoCount -= 1;
        countText.text = string.Format($"{undoCount}");
    }

    #endregion

    #region ExitGame
    public void EnterExitPet()
    {
        exitPetCount++;
        if (exitPetCount >= 2) ExitGame();
    }

    public void ExitPet()
    {
        exitPetCount--;
    }

    public void ExitGame()
    {
        StopListen();
        CameraSwitcher.SwitchCamera(defaultCam);
        ThirdPersonCameraControll.OnCrossHairMove(false);
        mazeCanvas.gameObject.SetActive(false);
        exitTrigger.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = true;
        playerMove.enabled = true;
    }

    #endregion

    #region Camera

    private void ChangeCam()
    {
        Pet p = PetManager.Instance.GetSelectPet;
        if (p.GetPetType == PetType.FIRE)
        {
            CameraSwitcher.SwitchCamera(fireCam);
        }
        else if (p.GetPetType == PetType.OIL)
        {
            CameraSwitcher.SwitchCamera(oilCam);
        }
    }

    #endregion
}
