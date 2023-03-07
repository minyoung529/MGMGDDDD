using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    LivingRoom = 0,
    Clock = 1,
    LoadingScene = 2,
    Count
}


public class SceneController : MonoBehaviour
{
    [SerializeField]
    private LoadingScene loadingScenePrefab;
    private static CanvasGroup loadGroup;
    private static LoadingScene loadingScene;

    public static Dictionary<SceneType, Action> OnEnterScene = new();
    public static Dictionary<SceneType, Action> OnExitScene = new();

    public static SceneType prevScene;
    private static SceneType curScene;
    public static SceneType CurrentScene => curScene;

    private void Start()
    {
        loadingScene = Instantiate(loadingScenePrefab);
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        loadGroup = loadingScene.GetComponent<CanvasGroup>();
        loadingScene.gameObject.SetActive(false);
    }

    public static void ChangeScene(SceneType sceneType)
    {
        Check(curScene, OnExitScene);
        OnExitScene[curScene]?.Invoke();
        prevScene = curScene;
        curScene = sceneType;

        loadingScene.gameObject.SetActive(true);
        loadGroup.DOFade(1f, 0.5f).OnComplete(() => loadingScene.ChangeScene());
    }

    public static void ChangeScene(AsyncOperation op)
    {
        loadGroup.DOFade(0f, 0.5f).OnComplete(() => loadingScene.gameObject.SetActive(false));
        Check(curScene, OnEnterScene);
        OnEnterScene[curScene]?.Invoke();
    }

    public static void ListeningEnter(SceneType sceneType, Action onEnter)
    {
        Check(sceneType, OnEnterScene);
        OnEnterScene[sceneType] += onEnter;
    }

    public static void ListningExit(SceneType sceneType, Action onExit)
    {
        Check(sceneType, OnExitScene);
        OnExitScene[sceneType] += onExit;
    }

    public static void StopListningEnter(SceneType sceneType, Action onEnter)
    {
        Check(sceneType, OnEnterScene);
        OnEnterScene[sceneType] -= onEnter;
    }

    public static void StopListningExit(SceneType sceneType, Action onExit)
    {
        Check(sceneType, OnExitScene);
        OnExitScene[sceneType] -= onExit;
    }

    private static void Check(SceneType type, Dictionary<SceneType, Action> map)
    {
        if (!map.ContainsKey(type))
        {
            map.Add(type, null);
        }
    }
}
