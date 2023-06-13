using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;

public class DropperController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onDropperStart;

    [SerializeField]
    private UnityEvent onDropperReset;

    [SerializeField]
    private UnityEvent onDropperClear;

    [SerializeField]
    private Transform playerSpawnPosition;

    private JumpMotion jumpMotion = new();

    [Header("PATTERN")]
    [SerializeField] private Transform patternRoot;
    private List<DropperPattern> patterns = new List<DropperPattern>();

    [Header("DECO")]
    [SerializeField]
    private List<ShaderOffsetMove> walls;

    [SerializeField]
    private Transform playerPosition;

    [SerializeField]
    private Transform topCameraPivot;

    [SerializeField]
    private List<ParticleSystem> particleSystems;

    [SerializeField]
    private ChangeRenderSettings renderSettings;

    [SerializeField]
    private GameObject pets;
    private ActiveChildTogglePosition changePetPos;

    private ThirdPersonCameraControll cameraController;

    private bool isPlaying = false;
    private bool isClear = false;

    private void Start()
    {
        cameraController = GameManager.Instance.PlayerController.GetComponent<ThirdPersonCameraControll>();
        jumpMotion.TargetPos = playerSpawnPosition.position;

        if (pets)
        {
            pets = Instantiate(pets);
            changePetPos = pets.GetComponent<ActiveChildTogglePosition>();
        }

        for (int i = 0; i < patternRoot.childCount; i++)
        {
            patterns.Add(patternRoot.GetChild(i).GetComponent<DropperPattern>());
            patterns[i].gameObject.SetActive(true);
        }

        DieTrigger[] triggers = patternRoot.GetComponentsInChildren<DieTrigger>();
        foreach (DieTrigger trigger in triggers)
        {
            trigger.OnDie += StopDropper;
        }

        patterns.ForEach(x =>
        {
            x.Init(playerPosition);
            x.gameObject.SetActive(false);
        });
    }

    [ContextMenu("Start Dropper")]
    public void StartDropper()
    {
        if (isClear) return;
        if (isPlaying) return;
        isPlaying = true;
        onDropperStart?.Invoke();

        // VISUAL
        renderSettings.Change();
        particleSystems.ForEach(x => x.Play());
        cameraController.InactiveCrossHair();
        DirectionalLightController.ChangeRotation(Quaternion.Euler(new Vector3(90f, 0f, 0f)), 1f);

        if (pets)
        {
            pets.gameObject.SetActive(true);
            pets.gameObject.transform.SetParent(GameManager.Instance.PlayerController.transform);
            pets.transform.localPosition = Vector3.zero;
            pets.transform.localRotation = Quaternion.identity;
            changePetPos.Open();
        }

        // FUNCTION

        walls.ForEach(x => x.Active());
        jumpMotion.StartJump(GameManager.Instance.PlayerController.transform, null, null, false, 2f);

        GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.Fall);
        GameManager.Instance.PlayerController.Move.IsBlockJump = true;
        PetManager.Instance.StopListen(InputAction.Pet_Follow);
        PetManager.Instance.InactivePetCanvas();
        
        StartCoroutine(StartPattern());
    }

    public void StopDropper()
    {
        if (isClear) return;
        ResetDropperData();
    }

    public void Clear()
    {
        PetManager.Instance.ActivePetCanvas();

        onDropperClear?.Invoke();

        GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.DefaultMove);
        isClear = true;
        GameManager.Instance.PlayerController.Move.IsBlockJump = false;
        PetManager.Instance.StartListen(InputAction.Pet_Follow);
    }

    private void ResetDropperData()
    {
        if (!isPlaying) return;
        StopAllCoroutines();
        patterns.ForEach(x => x.Pause());

        StartCoroutine(DelayReset());
    }

    private IEnumerator DelayReset()
    {
        isPlaying = false;

        renderSettings.Back();
        walls.ForEach(x => x.Inactive());
        onDropperReset?.Invoke();

        particleSystems.ForEach(x => x.Stop());
        patterns.ForEach(x => x.ResetPattern());

        yield return new WaitForSeconds(1f);
        walls.ForEach(x => x.Active());

        if (pets)
        {
            changePetPos.ForceClosePos();
        }

        yield return new WaitForSeconds(1.5f);

        StartDropper();
    }

    private IEnumerator StartPattern()
    {
        List<int> shuffles = new();

        for (int i = 0; i < patterns.Count; i++)
            shuffles.Add(i);

        for (int i = 0; i < patterns.Count * 2; i++)
        {
            int idx1 = Random.Range(0, patterns.Count);
            int idx2 = Random.Range(0, patterns.Count);

            int temp = shuffles[idx1];
            shuffles[idx1] = shuffles[idx2];
            shuffles[idx2] = temp;
        }

        yield return new WaitForSeconds(1.75f);

        for (int i = 0; i < patterns.Count; i++)
        {
            yield return new WaitForSeconds(1f);

            patterns[shuffles[i]].PatternStart();
            yield return new WaitForSeconds(0.25f);
            topCameraPivot.DOShakePosition(4f, 0.7f);
            yield return new WaitForSeconds(1.2f);
            patterns[shuffles[i]].ExitPatternAnimation();
        }

        Clear();
    }
}
