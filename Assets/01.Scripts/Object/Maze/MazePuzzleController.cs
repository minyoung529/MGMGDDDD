using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazePuzzleController : MonoSingleton<MazePuzzleController>
{
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;

    [SerializeField] CinemachineFreeLook defaultCam;
    [SerializeField] CinemachineVirtualCamera fireCam;
    [SerializeField] CinemachineVirtualCamera oilCam;

    [SerializeField] Canvas crosshairCanvas;
    [SerializeField] Text countText;

    private const int maxUndoCount = 2;
    private bool crossHairMove = false;
    private int exitPetCount = 0;
    private int undoCount = 2;

    private RectTransform crosshair;
    Stack<MazeButton> buttons = new Stack<MazeButton>();

    private void Start()
    {
        crosshair = crosshairCanvas.transform.GetChild(0).GetComponent<RectTransform>();
        RegisterCamera();
        ResetController();
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Down_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_First_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_Second_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_Third_Pet, SwitchPet);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Up_Pet, SwitchPet);
        InputManager.StopListeningInput(InputAction.Down_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_First_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_Second_Pet, SwitchPet);
        InputManager.StartListeningInput(InputAction.Select_Third_Pet, SwitchPet);
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
        OnCrossHairMove(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = false;
    }


    public void OnCrossHairMove(bool value)
    {
        if (crossHairMove == value) return;

        crossHairMove = value;
        if (value == true) StartCoroutine(CrossHairMove());
    }

    private IEnumerator CrossHairMove()
    {
        while (true)
        {
            if (!crossHairMove) break;
            crosshair.transform.position = Input.mousePosition;
            yield return new WaitForSeconds(0.01f);
        }
        crosshair.anchoredPosition = new Vector3(0, 0, 0);
        yield return null;
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
        exitEvent?.Invoke();

        StopListen();
        CameraSwitcher.SwitchCamera(defaultCam);
        OnCrossHairMove(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Pet.IsCameraAimPoint = true;
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
