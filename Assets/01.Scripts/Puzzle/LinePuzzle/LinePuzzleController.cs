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

    private OilPet oilPet;
    // Todo: Later > Change Access Method
    private OilSkillState oilSkillState;

    private int idx = 0;

    [SerializeField]
    private Transform trigger;

    [SerializeField]
    private ParticleSystem oilTeleportParticle;

    private bool isPlaying = false;

    [Header("EVENTS")]
    [SerializeField] private UnityEvent onEnterGame;
    [SerializeField] private UnityEvent onExitGame;

    public CinemachineVirtualCameraBase topCamera;

    #region Property
    public bool isOilMove = false;
    public bool IsPainting { get; private set; } = false;
    public List<PlatformPiece> SelectedPieces { get; set; } = new List<PlatformPiece>();
    public PlatformPiece SelectedPiece => (SelectedPieces.Count > 0) ? SelectedPieces[0] : null;
    public PlatformPiece EndPiece => (SelectedPieces.Count > 0) ? SelectedPieces[SelectedPieces.Count - 1] : null;
    #endregion

    private void Awake()
    {
        cameraController = FindObjectOfType<ThirdPersonCameraControll>();
        topCamera = GetComponentInChildren<CinemachineVirtualCameraBase>();
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

        if(Input.GetKeyDown(KeyCode.G))
        {
            GetNextPuzzle();
        }

        trigger.transform.position = GameManager.Instance.GetMousePos();
    }

    private void ResetBoard()
    {
        oilPet.SkillState.SkillData.ClearOil();
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

        PetSetting();
        StartGame();
    }

    private void PetSetting()
    {
        oilPet ??= PetManager.Instance.GetPetByKind<OilPet>() as OilPet;
        oilSkillState ??= oilPet.GetComponentInChildren<OilSkillState>();
        Pet firePet = PetManager.Instance.GetPetByKind<FirePet>();

        oilPet.SetForcePosition(oilSpawnPosition.position);
        firePet.SetForcePosition(fireSpawnPosition.position);

        oilPet.SetTarget(null);
        firePet.SetTarget(null);

        Pet.IsCameraAimPoint = false;

        oilPet.SkillState.IsDirectSpread = false;
        oilPet.SkillState.OnEndSkill += MoveToPortal;
        oilPet.SkillState.OnStartSkill += StartPaintingOil;
        oilPet.SkillState.SkillData.IsCheckDistance = false;
    }

    public void InsertCurrentPiece(PlatformPiece piece)
    {
        int count = SelectedPieces.Count;
        if (SelectedPieces.Count == 0 || SelectedPieces[count - 1] != piece)
        {
            SelectedPieces.Add(piece);
        }
    }

    public void ExitGame()
    {
        GameManager.Instance.SetCursorVisible(true);
        isPlaying = false;

        cameraController.ActiveCrossHair();
        OilPetSkill.IsCrosshair = true;
        oilPet.SkillState.IsDirectSpread = true;

        oilPet.SkillState.OnEndSkill -= MoveToPortal;
        oilPet.SkillState.OnStartSkill -= StartPaintingOil;
        oilPet.SkillState.SkillData.IsCheckDistance = true;

        onExitGame?.Invoke();
    }

    private void StartGame()
    {
        linePuzzles[idx].StartGame(GetIsOilMove);
    }

    private bool GetIsOilMove()
    {
        return isOilMove;
    }

    private void MoveToPortal()
    {
        IsPainting = false;

        // player draw invalid line => kill skill and change idle state
        if (InvalidLine())
        {
            oilSkillState.KillSkill();
            oilPet.State.ChangeState((int)PetStateName.Idle);
            return;
        }

        ConnectionPortal portal = CurrentPuzzle.OilPortals[SelectedPiece.Index];

        oilPet.Event.StartListening((int)PetEventName.OnSetDestination, ChangeState);
        oilPet.SetDestination(portal.transform.position, stopDistance: 0);
        oilPet.State.ChangeState((int)PetStateName.Move);

        oilPet.Event.StartListening((int)PetEventName.OnArrive, ForceMoveBoard);
    }

    private void ForceMoveBoard()
    {
        isOilMove = true;
        oilPet.SetForcePosition(oilPet.SkillState.SkillStartPoint);
        oilTeleportParticle.transform.position = oilPet.SkillState.SkillStartPoint;
        oilTeleportParticle.Play();

        oilPet.SkillState.SkillData.OnEndSpread_Once += OilBack;
        oilPet.SkillState.SpreadOil();

        oilPet.Event.StopListening((int)PetEventName.OnArrive, ForceMoveBoard);
    }

    private void ChangeState()
    {
        oilPet.State.ChangeState((int)PetStateName.Move);
        oilPet.Event.StopListening((int)PetEventName.OnSetDestination, ChangeState);
    }

    private void OilBack()
    {
        oilPet.SetForcePosition(oilSpawnPosition.position);
        oilTeleportParticle.transform.position = oilSpawnPosition.position;
        oilTeleportParticle.Play();

        isOilMove = false;
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

        seq.AppendCallback(() => linePuzzles[idx].StartGame(GetIsOilMove));
        SelectedPieces?.Clear();
    }

    private void StartPaintingOil()
    {
        SelectedPieces.Clear();
        IsPainting = true;
        CurrentPuzzle.ResetOil();
    }

    private void EndPuzzle()
    {
        ExitGame();
        oilPet.SkillState.PauseSkill(false);
    }

    public void PauseOilPet(bool pause)
    {
        oilPet.SkillState.PauseSkill(pause);
    }

    private bool InvalidLine()
    {
        bool isInvalidLine = SelectedPiece == null || EndPiece == null; // Is Null

        if (isInvalidLine)
            return true;

        isInvalidLine |= SelectedPiece.Index < 0 || CurrentPuzzle.OilPortals.Count <= SelectedPiece.Index;  // Invalid Index
        isInvalidLine |= (EndPiece.Index != SelectedPiece.Index); // Is Different Node (EndPiece, SelectedPiece)
        isInvalidLine |= (EndPiece == SelectedPiece); // Is Same Node
        isInvalidLine |= SelectedPieces.FindAll(x => EndPiece.Index != x.Index && x.Index >= 0).Count > 0; // Selected Nodes Count have to be two

        return isInvalidLine;
    }
}
