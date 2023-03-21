using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LinePuzzleController : MonoBehaviour
{
    [SerializeField]
    private LinePuzzle[] linePuzzles;
    private LinePuzzle CurrentPuzzle => linePuzzles[idx];

    [SerializeField]
    private CinemachineVirtualCamera cmVcam;

    private ThirdPersonCameraControll cameraController;

    [SerializeField]
    private Transform oilSpawnPosition;

    [SerializeField]
    private Transform fireSpawnPosition;

    private FirePet firePet;
    private OilPet oilPet;

    private int idx = 0;

    [SerializeField]
    private Transform trigger;

    public static PlatformPiece CurrentPiece { get; set; }

    private void Awake()
    {
        cameraController = FindObjectOfType<ThirdPersonCameraControll>();
        firePet = FindObjectOfType<FirePet>();
        oilPet = FindObjectOfType<OilPet>();

        InputManager.StartListeningInput(InputAction.Pet_Skill, Select);
    }

    private void Start()
    {
        CameraSwitcher.UnRegister(cmVcam);
        CameraSwitcher.Register(cmVcam);
    }

    public void EnterGame()
    {
        CameraSwitcher.SwitchCamera(cmVcam);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        cameraController.InactiveCrossHair();
        OilPetSkill.IsCrosshair = false;

        oilPet.SetForcePosition(oilSpawnPosition.position);
        firePet.SetForcePosition(fireSpawnPosition.position);

        Pet.IsCameraAimPoint = false;

        oilPet.IsDirectSpread = false;
        oilPet.OnEndSkill += AutoMoveOil;
        oilPet.OnStartSkill += CurrentPuzzle.ResetOil;

        StartGame();
    }

    public void ExitGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraController.ActiveCrossHair();
        OilPetSkill.IsCrosshair = true;
        oilPet.IsDirectSpread = true;
        Pet.IsCameraAimPoint = true;

        oilPet.OnEndSkill -= AutoMoveOil;
        oilPet.OnStartSkill -= CurrentPuzzle.ResetOil;

    }

    private void StartGame()
    {
        linePuzzles[idx].StartGame();
    }

    public void ClearGame()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Select);
    }

    private void Select(InputAction action, float value)
    {
        trigger.transform.position = GameManager.Instance.GetMousePos();
    }

    private void AutoMoveOil()
    {
        if (CurrentPiece)
        {
            ConnectionPortal portal = CurrentPuzzle.OilPortals[CurrentPiece.Index];
            oilPet.MovePoint(portal.transform.position);
            oilPet.OnEndPointMove += ForceMoveBoard;
        }
    }

    private void ForceMoveBoard()
    {
        oilPet.SetForcePosition(oilPet.OilPetSkill.StartPoint);

        oilPet.OilPetSkill.OnEndSpread_Once += () => oilPet.SetForcePosition(oilSpawnPosition.position);
        oilPet.SpreadOil();
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Select);
    }
}
