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

    private bool crossHairMove = false;

    private RectTransform crosshair;
    Stack<MazeButton> buttons = new Stack<MazeButton>();

    private void Start()
    {
        crosshair = crosshairCanvas.transform.GetChild(0).GetComponent<RectTransform>();
        RegisterCamera();
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
        StartCoroutine(DelayOneFrame());
    }

    private IEnumerator DelayOneFrame()
    {
        yield return null;

        if ((PetManager.Instance.GetSelectPet.GetPetType == PetType.FirePet || PetManager.Instance.GetPetByKind<OilPet>() == null)) yield return null;
        else
        {
            if (PetManager.Instance.GetSelectPet.GetPetType == PetType.OilPet)
                CameraSwitcher.SwitchCamera(oilCam);
            if (PetManager.Instance.GetSelectPet.GetPetType == PetType.FirePet)
                CameraSwitcher.SwitchCamera(fireCam);
        }
    }

    private void RegisterCamera()
    {
        CameraSwitcher.Register(oilCam);
        CameraSwitcher.Register(fireCam);
    }

    #region EnterGame

    public void EnterGame()
    {
        if (PetManager.Instance.PetCount < 2) return;

        for (int i = 0; i < PetManager.Instance.PetCount; i++)
        {
            PetManager.Instance.GetPetList[i].State.ChangeState((int)PetStateName.Idle);
        }
        PetManager.Instance.AllPetActions(x => x.AgentAcceleration = 80);

        enterEvent?.Invoke();
        StartListen();
        ChangeCam();
        OnCrossHairMove(true);

        GameManager.Instance.SetCursorVisible(true);
        Cursor.visible = false;
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
