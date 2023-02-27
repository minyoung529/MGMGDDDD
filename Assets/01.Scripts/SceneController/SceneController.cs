using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LivingRoom,
    NewClock_QU,
    LoadingScene,
    Count
}


public class SceneController
{
    public static Dictionary<SceneType, Action> OnEnterScene = new();
    public static Dictionary<SceneType, Action> OnExitScene = new();

    private static SceneType curScene;
    public static SceneType CurrentScene => curScene;

    public static void ChangeScnee(SceneType sceneType)
    {
        Check(curScene, OnExitScene);
        OnExitScene[curScene]?.Invoke();
        curScene = sceneType;
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());
    }

    public static void ChangeScene(AsyncOperation op)
    {
        Debug.Log("EnterScene");
        Check(curScene, OnEnterScene);
        OnEnterScene[curScene]?.Invoke();
    }

    public static void ListningEnter(SceneType sceneType, Action onEnter)
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
        if(!map.ContainsKey(type))
        {
            map.Add(type, null);
        }
    }
}
