using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinePuzzleController : MonoBehaviour
{
    [SerializeField]
    private LinePuzzle[] linePuzzles;
    private LinePuzzle CurrentPuzzle => linePuzzles[idx];

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

    private bool isPlaying = false;

    [Header("EVENTS")]
    [SerializeField] private UnityEvent onEnterGame;
    [SerializeField] private UnityEvent onExitGame;

    public CinemachineVirtualCameraBase topCamera;

    #region Property
    public static PlatformPiece CurrentPiece { get; set; }
    public static bool IsOilMove { get; set; } = false;
    public static PlatformPiece SelectedPiece { get; set; }
    public static PlatformPiece EndPiece { get; set; }
    #endregion

    private void Awake()
    {
        cameraController = FindObjectOfType<ThirdPersonCameraControll>();
        topCamera = GetComponentInChildren<CinemachineVirtualCameraBase>();
        firePet = FindObjectOfType<FirePet>();
        oilPet = FindObjectOfType<OilPet>();
    }

    private void Start()
    {
        foreach (LinePuzzle puzzle in linePuzzles)
        {
            puzzle.OnClear += GetNextPuzzle;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetBoard();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CurrentPuzzle.EndPuzzle();
            GetNextPuzzle();
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
        if (PetManager.Instance.PetCount < 2) return;

        isPlaying = true;
        onEnterGame?.Invoke();

        GameManager.Instance.SetCursorVisible(true);

        cameraController.InactiveCrossHair();
        OilPetSkill.IsCrosshair = false;

        oilPet.SetForcePosition(oilSpawnPosition.position);
        firePet.SetForcePosition(fireSpawnPosition.position);

        Pet.IsCameraAimPoint = false;

        oilPet.IsDirectSpread = false;
        oilPet.OnEndSkill += SetEndPiece;
        oilPet.OnEndSkill += MoveToPortal;
        oilPet.OnStartSkill += ResetOil;
        oilPet.OnStartSkill += SetSelectedColor;
        oilPet.OilPetSkill.IsCheckDistance = false;

        StartGame();
    }

    private void SetSelectedColor()
    {
        if (CurrentPiece)
            SelectedPiece = CurrentPiece;
    }

    private void SetEndPiece()
    {
        if (CurrentPiece)
            EndPiece = CurrentPiece;
    }

    public void ExitGame()
    {
        GameManager.Instance.SetCursorVisible(true);
        isPlaying = false;

        cameraController.ActiveCrossHair();
        OilPetSkill.IsCrosshair = true;
        oilPet.IsDirectSpread = true;

        oilPet.OnEndSkill -= MoveToPortal;
        oilPet.OnEndSkill -= SetEndPiece;
        oilPet.OnStartSkill -= ResetOil;
        oilPet.OnStartSkill -= SetSelectedColor;
        oilPet.OilPetSkill.IsCheckDistance = true;

        onExitGame?.Invoke();
    }

    private void StartGame()
    {
        linePuzzles[idx].StartGame();
    }

    private void MoveToPortal()
    {
        if (SelectedPiece == null || EndPiece == null) return;
        if (SelectedPiece.Index < 0 || CurrentPuzzle.OilPortals.Count <= SelectedPiece.Index) return;
        if (EndPiece.Index != SelectedPiece.Index) return;

        ConnectionPortal portal = CurrentPuzzle.OilPortals[SelectedPiece.Index];
        oilPet.SetDestination(portal.transform.position);
        oilPet.OnArrive += ForceMoveBoard;
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
        seq.AppendCallback(() =>
        {
            if (idx - 1 >= 0)
                linePuzzles[idx - 1].gameObject.SetActive(false);
        });
        seq.Append(linePuzzles[idx].transform.DOMove(linePuzzles[0].transform.position, 1f));
        seq.Join(topCamera.transform.DOShakePosition(1.2f, 0.75f));

        seq.AppendCallback(() => linePuzzles[idx].StartGame());
    }

    private void ResetOil()
    {
        CurrentPuzzle.ResetOil();
    }

    private void EndPuzzle()
    {
        ExitGame();
        oilPet.PauseSkill(false);
    }

    public void PauseOilPet(bool pause)
    {
        oilPet.PauseSkill(pause);
    }
}
