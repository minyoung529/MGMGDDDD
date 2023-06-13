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
    [SerializeField] UnityEvent clearEvent;

    [SerializeField] CinemachineFreeLook defaultCam;
    [SerializeField] CinemachineVirtualCamera fireCam;
    [SerializeField] CinemachineVirtualCamera oilCam;

    [SerializeField] Canvas crosshairCanvas;
    [SerializeField] Text countText;

    private const int maxUndoCount = 2;
    private bool crossHairMove = false;
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
        if(PetManager.Instance.GetSelectPet.GetPetType == PetType.FirePet)
        {
            CameraSwitcher.SwitchCamera(fireCam);
        }
        else if(PetManager.Instance.GetSelectPet.GetPetType == PetType.OilPet)
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
        countText.text = string.Format($"{undoCount}");
    }

    #region EnterGame

    public void EnterGame()
    {
        if (PetManager.Instance.PetCount < 2) return;

        enterEvent?.Invoke();
        StartListen();
        ChangeCam();
        OnCrossHairMove(true);

        GameManager.Instance.SetCursorVisible(true);
        PetManager.Instance.AllPetActions(x => x.AgentAcceleration = 80);
    }

    public void StopPetAction()
    {
        PetManager.Instance.StopListen(InputAction.Pet_Follow);
        PetManager.Instance.StopListen(InputAction.Pet_Skill);
        PetManager.Instance.StopListen(InputAction.Pet_Skill_Up);
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

    [ContextMenu("Clear")]
    public void ClearMaze()
    {
        clearEvent?.Invoke();

        StopListen();
        CameraSwitcher.SwitchCamera(defaultCam);
        OnCrossHairMove(false);

        GameManager.Instance.SetCursorVisible(false);
        PetManager.Instance.AllPetActions(x => x.ResetAgentValue());
        PetManager.Instance.StartListen(InputAction.Pet_Follow);
        PetManager.Instance.StartListen(InputAction.Pet_Skill);
        PetManager.Instance.StartListen(InputAction.Pet_Skill_Up);
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

   

    #region Camera

    public void ChangeCam()
    {
        Pet p = PetManager.Instance.GetSelectPet;
        
        if (p.GetPetType == PetType.FirePet)
        {
            CameraSwitcher.SwitchCamera(fireCam);
        }
        else if (p.GetPetType == PetType.OilPet)
        {
            CameraSwitcher.SwitchCamera(oilCam);
        }
    }

    #endregion
}
