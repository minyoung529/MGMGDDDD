using Cinemachine;
using DG.Tweening;
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

    [SerializeField]
    private Transform boardTransform;

    [SerializeField]
    private ParticleSystem oilTeleportParticle;

    public static PlatformPiece CurrentPiece { get; set; }

    public static bool IsOilMove { get; set; } = false;

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

        foreach(LinePuzzle puzzle in linePuzzles)
        {
            puzzle.OnClear += ClearPuzzle;
            puzzle.OnFire += BuildAllMesh;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ResetBoard();
        }
    }

    private void ResetBoard()
    {
        oilPet.OilPetSkill.ClearOil();
        CurrentPuzzle.ResetPuzzle();
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
        oilPet.OnEndSkill += MoveToPortal;
        oilPet.OnStartSkill += CurrentPuzzle.ResetOil;
        oilPet.OilPetSkill.IsCheckDistance = false;

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

        oilPet.OnEndSkill -= MoveToPortal;
        oilPet.OnStartSkill -= CurrentPuzzle.ResetOil;
        oilPet.OilPetSkill.IsCheckDistance = true;
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

    private void MoveToPortal()
    {
        if (CurrentPiece)
        {
            if (CurrentPiece.Index < 0 || CurrentPuzzle.OilPortals.Count <= CurrentPiece.Index) return;

            ConnectionPortal portal = CurrentPuzzle.OilPortals[CurrentPiece.Index];
            oilPet.SetDestination(portal.transform.position);
            oilPet.onArrive += ForceMoveBoard;
        }
    }

    private void ForceMoveBoard()
    {
        IsOilMove = true;
        oilPet.SetForcePosition(oilPet.OilPetSkill.StartPoint);
        oilTeleportParticle.transform.position = oilPet.OilPetSkill.StartPoint;
        oilTeleportParticle.Play();

        oilPet.OilPetSkill.OnEndSpread_Once += OilBack;
        oilPet.SpreadOil();
    }

    private void OilBack()
    {
        oilPet.SetForcePosition(oilSpawnPosition.position);
        oilTeleportParticle.transform.position = oilSpawnPosition.position;
        oilTeleportParticle.Play();

        IsOilMove = false;
    }

    private void ClearPuzzle()
    {
        if (++idx >= linePuzzles.Length)
        {
            EndPuzzle();
            return;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(2f);
        seq.Append(linePuzzles[idx].transform.DOMove(boardTransform.position, 1f));
        seq.AppendCallback(() => linePuzzles[idx].StartGame());
    }

    private void EndPuzzle()
    {

    }

    [ContextMenu("Dynamic Build All Mesh")]
    public void BuildAllMesh()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(8f);
        seq.AppendCallback(() =>
        {
            foreach (LinePuzzle puzzle in linePuzzles)
            {
                puzzle.BuildAllMesh();
            }
        });
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Pet_Skill, Select);
    }
}
