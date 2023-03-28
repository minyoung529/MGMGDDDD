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

    private bool isPlaying = false;

    private void Awake()
    {
        cameraController = FindObjectOfType<ThirdPersonCameraControll>();
        firePet = FindObjectOfType<FirePet>();
        oilPet = FindObjectOfType<OilPet>();
    }

    private void Start()
    {
        CameraSwitcher.UnRegister(cmVcam);
        CameraSwitcher.Register(cmVcam);

        foreach(LinePuzzle puzzle in linePuzzles)
        {
            puzzle.OnClear += GetNextPuzzle;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        if(Input.GetKeyDown(KeyCode.F))
        {
            ResetBoard();
        }

        trigger.transform.position = GameManager.Instance.GetMousePos();
    }

    private void ResetBoard()
    {
        oilPet.OilPetSkill.ClearOil();
        CurrentPuzzle.ResetPuzzle();
    }

    public void EnterGame()
    {
        isPlaying = true;
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
        isPlaying = false;

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

    private void MoveToPortal()
    {
        if (CurrentPiece)
        {
            if (CurrentPiece.Index < 0 || CurrentPuzzle.OilPortals.Count <= CurrentPiece.Index) return;

            ConnectionPortal portal = CurrentPuzzle.OilPortals[CurrentPiece.Index];
            oilPet.SetDestination(portal.transform.position);
            oilPet.onArrive += ForceMoveBoard;
        }
        else
        {
            Debug.Log("CurrentPiece가 NULL입니다.");
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

    private void GetNextPuzzle()
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
        EnterGame();
        oilPet.PauseSkill(false);
    }

    public void PauseOilPet(bool pause)
    {
        oilPet.PauseSkill(pause);
    }
}
