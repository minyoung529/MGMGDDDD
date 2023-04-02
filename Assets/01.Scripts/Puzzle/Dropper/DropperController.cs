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

    private Animator player;

    private JumpMotion jumpMotion = new();

    [Header("PATTERN")]
    [SerializeField] private Transform patternRoot;
    private List<DropperPattern> patterns;

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

    private ThirdPersonCameraControll cameraController;

    private bool isPlaying = false;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().GetComponent<Animator>();
        cameraController = player.GetComponentInChildren<ThirdPersonCameraControll>();
        jumpMotion.TargetPos = playerSpawnPosition.position;

        patterns = patternRoot.GetComponentsInChildren<DropperPattern>().ToList();

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
        if (isPlaying) return;
        isPlaying = true;
        onDropperStart?.Invoke();

        // VISUAL
        renderSettings.Change();
        particleSystems.ForEach(x => x.Play());
        cameraController.InactiveCrossHair();
        DirectionalLightController.ChangeRotation(Quaternion.Euler(new Vector3(90f, 0f, 0f)), 1f);

        // FUNCTION
        walls.ForEach(x => x.Active());
        jumpMotion.StartJump(player.transform, null, null, false, 2f);

        player.SetTrigger("tStateChange");
        player.SetInteger("iStateNum", (int)StateName.Fall);
        StartCoroutine(StartPattern());
    }

    public void StopDropper()
    {
        ResetDropperData();
    }

    public void Clear()
    {
        ResetDropperData();
        onDropperClear?.Invoke();
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
        yield return new WaitForSecondsRealtime(1f);
        isPlaying = false;

        renderSettings.Back();
        walls.ForEach(x => x.Inactive());
        onDropperReset?.Invoke();

        particleSystems.ForEach(x => x.Stop());
        patterns.ForEach(x => x.ResetPattern());

        cameraController.ActiveCrossHair();

        // TEST
        CameraSwitcher.SwitchCamera(player.GetComponentInChildren<CinemachineVirtualCameraBase>());

        player.SetTrigger("tStateChange");
        player.SetInteger("iStateNum", (int)StateName.DefaultMove);
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
            topCameraPivot.DOShakePosition(Random.Range(2f, 2.5f), Random.Range(0.5f, 1.2f));
            yield return new WaitForSeconds(1.6f);
            patterns[shuffles[i]].ExitPatternAnimation();
        }

        Clear();
    }
}
