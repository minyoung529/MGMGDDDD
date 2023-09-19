using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LivingRoom = 0,
    Lobby_FirstFloor = 1,
    Clock_Lobby = 2,
    SecondScene = 3,
    StartScene = 4,
    Boss = 5,
    EndingCutscene = 6,
    Count
}


public class SceneController : MonoBehaviour
{
    [SerializeField]
    private LoadingScene loadingScenePrefab;
    private static LoadingScene loadingScene;

    public static Dictionary<SceneType, Action> OnEnterScene = new();
    public static Dictionary<SceneType, Action> OnExitScene = new();

    public static SceneType prevScene;
    private static SceneType curScene;
    public static SceneType CurrentScene => curScene;

    public static bool IsChanging { get; private set; }

    private void Start()
    {
        loadingScene = Instantiate(loadingScenePrefab);
        loadingScene.gameObject.SetActive(false);
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        if (OnEnterScene.Count == 0)
        {
            SceneManager.activeSceneChanged += EnterCurrentScene;
        }

        for (int i = 0; i < (int)SceneType.Count; i++)
        {
            if (!OnEnterScene.ContainsKey((SceneType)i))
            {
                OnEnterScene.Add((SceneType)i, null);
            }
        }
    }

    private void EnterCurrentScene(Scene prev, Scene cur)
    {
        OnEnterScene[curScene]?.Invoke();
        IsChanging = false;
    }
    public static void ChangeScene(SceneType sceneType, bool isLoading = true)
    {
        if (IsChanging) return;

        IsChanging = true;
        Check(curScene, OnExitScene);
        OnExitScene[curScene]?.Invoke();
        prevScene = curScene;
        curScene = sceneType;

        if (isLoading)
        {
            loadingScene.ChangeScene();
        }
        else
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneType.ToString());
            Check(curScene, OnEnterScene);
            op.completed += ChangeScene;
        }
    }

    public static void ChangeScene(AsyncOperation op)
    {
        loadingScene.InactiveScene();
        Check(curScene, OnEnterScene);
        OnEnterScene[curScene]?.Invoke();

        IsChanging = false;
    }

    public static void ListeningEnter(SceneType sceneType, Action onEnter)
    {
        Check(sceneType, OnEnterScene);
        OnEnterScene[sceneType] += onEnter;
    }

    #region ALL
    public static void ListeningEnter(Action onEnter)
    {
        List<SceneType> scenes = Keys(OnEnterScene.Keys);

        foreach (var key in scenes)
        {
            OnEnterScene[key] += onEnter;
        }
    }

    public static void ListeningExit(Action onExit)
    {
        List<SceneType> scenes = Keys(OnEnterScene.Keys);

        foreach (var key in scenes)
            OnExitScene[key] += onExit;
    }

    public static void StopListeningEnter(Action onEnter)
    {
        List<SceneType> scenes = Keys(OnEnterScene.Keys);

        foreach (var key in scenes)
            OnEnterScene[key] -= onEnter;
    }

    public static void StopListeningExit(Action onExit)
    {
        List<SceneType> scenes = Keys(OnEnterScene.Keys);

        foreach (var key in scenes)
            OnExitScene[key] -= onExit;
    }
    #endregion

    public static void ListningExit(SceneType sceneType, Action onExit)
    {
        Check(sceneType, OnExitScene);
        OnExitScene[sceneType] += onExit;
    }

    public static void StopListeningEnter(SceneType sceneType, Action onEnter)
    {
        Check(sceneType, OnEnterScene);
        OnEnterScene[sceneType] -= onEnter;
    }

    public static void StopListeningExit(SceneType sceneType, Action onExit)
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

    private static List<SceneType> Keys(Dictionary<SceneType, Action>.KeyCollection keys)
    {
        return new List<SceneType>(keys);
    }
}
