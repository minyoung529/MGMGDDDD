using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LinePuzzleController : MonoBehaviour
{
    [SerializeField]
    private LinePuzzle[] linePuzzles;
    private LinePuzzle CurrentPuzzle => linePuzzles[idx];

    private ThirdPersonCameraControll cameraController;

    [SerializeField]
    private Text roundText;

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
    [SerializeField] private UnityEvent onClearRound;

    public CinemachineVirtualCameraBase topCamera;
    #region Property
    public bool isOilMove = false;
    public bool IsPainting { get; private set; } = false;
    public List<PlatformPiece> SelectedPieces { get; set; } = new List<PlatformPiece>();
    public PlatformPiece SelectedPiece => (SelectedPieces.Count > 0) ? SelectedPieces[0] : null;
    public PlatformPiece EndPiece => (SelectedPieces.Count > 0) ? SelectedPieces[SelectedPieces.Count - 1] : null;

    public List<PlatformPiece> BurningPieces { get; set; } = new List<PlatformPiece>();
    private bool isBurning = false;
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

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetNextPuzzle();
        }
#endif

        trigger.transform.position = GameManager.Instance.GetMousePos();
        CheckBurning();
    }

    public void StartListenReset()
    {
        InputManager.StartListeningInput(InputAction.Interaction, ResetBoard);
    }

    public void StopListenReset()
    {
        InputManager.StopListeningInput(InputAction.Interaction, ResetBoard);
    }

    private void CheckBurning()
    {
        if (!isBurning && BurningPieces.Count > 0)
        {
            isBurning = true;
            PetManager.Instance.StopListen(InputAction.Pet_Skill);
            PetManager.Instance.StopListen(InputAction.Pet_Skill_Up);
        }

        else if (isBurning && BurningPieces.Count == 0)
        {
            isBurning = false;
            PetManager.Instance.StartListen(InputAction.Pet_Skill);
            PetManager.Instance.StartListen(InputAction.Pet_Skill_Up);
        }
    }

    private void ResetBoard(InputAction action = InputAction.Interaction, float value = 0f)
    {
        CurrentPuzzle.ResetPuzzle();
        isOilMove = false;

        SelectedPieces.Clear();
        BurningPieces.Clear();

        oilSkillState.SkillData.ClearOil();
        oilSkillState.KillSkill();
    }

    [ContextMenu("ENTER GAME")]
    public void EnterGame()
    {
        if (PetManager.Instance.PetCount < 2) return;

        for (int i = 0; i < linePuzzles.Length; i++)
        {
            linePuzzles[i].gameObject.SetActive(true);
        }

        isPlaying = true;
        onEnterGame?.Invoke();

        GameManager.Instance.SetCursorVisible(true);
        cameraController.InactiveCrossHair();
        OilPetSkill.IsCrosshair = false;
        GameManager.Instance.PlayerController.Move.LockInput();
        PetManager.Instance.StopListen(InputAction.Pet_Follow);
        PetManager.Instance.InactivePetCanvas();

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
        GameManager.Instance.SetCursorVisible(false);
        isPlaying = false;

        cameraController.ActiveCrossHair();
        OilPetSkill.IsCrosshair = true;
        oilPet.SkillState.IsDirectSpread = true;

        oilPet.SkillState.OnEndSkill -= MoveToPortal;
        oilPet.SkillState.OnStartSkill -= StartPaintingOil;
        oilPet.SkillState.SkillData.IsCheckDistance = true;
        GameManager.Instance.PlayerController.Move.UnLockInput();
        PetManager.Instance.StartListen(InputAction.Pet_Follow);
        PetManager.Instance.ActivePetCanvas();

        onExitGame?.Invoke();
    }

    private void StartGame()
    {
        roundText.text = $"ROUND 1";
        linePuzzles[idx].StartGame(GetIsOilMove);
    }

    private bool GetIsOilMove(int idx = -1)
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
            SelectedPieces.Clear();
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
        SelectedPieces.Clear();

        isOilMove = false;
    }

    [ContextMenu("GET NEXT PUZZLE")]
    public void GetNextPuzzle()
    {
        if (++idx >= linePuzzles.Length)
        {
            EndPuzzle();
            return;
        }

        onClearRound?.Invoke();
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
        seq.AppendCallback(() => roundText.text = $"ROUND {idx + 1}");
        SelectedPieces?.Clear();
    }

    private void StartPaintingOil()
    {
        // SelectedPieces.Clear();
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
        isInvalidLine |= SelectedPieces.FindAll(x => x.IsDestroyed).Count > 0;

        return isInvalidLine;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        for (int i = 1; i < linePuzzles.Length; i++)
        {
            linePuzzles[i].gameObject.SetActive(false);
        }

        linePuzzles[0].gameObject.SetActive(true);

        int boardCount = linePuzzles[0].BoardCount;
        int powBoardCount = Mathf.RoundToInt(Mathf.Pow((float)linePuzzles[0].BoardCount, 2f));

        for (int i = 0; i < powBoardCount; i++)
        {
            Transform board = linePuzzles[0][i].transform;
            Vector3 pos = board.position;

            if (i % boardCount >= boardCount / 2) // right
            {
                board.position += board.right * 10f;
            }
            else
            {
                board.position -= board.right * 10f;
            }

            board.DOMove(pos, 0.65f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                for (int i = 1; i < linePuzzles.Length; i++)
                {
                    linePuzzles[i].gameObject.SetActive(true);
                }
            });
        }
    }
}
