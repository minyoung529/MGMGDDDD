using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LivingRoom,
    NewClock
}


public class SceneController
{
    public static Dictionary<SceneType, Action> OnEnterScene;
    public static Dictionary<SceneType, Action> OnExitScene;

    private static SceneType curScene;
    public static SceneType CurrentScene => curScene;

    public static void ChangeScnee(SceneType sceneType)
    {
        OnExitScene[curScene]?.Invoke();
        curScene = sceneType;
        OnEnterScene[curScene]?.Invoke();
        SceneManager.LoadScene(sceneType.ToString());
    }

    public static void ListningEnter(SceneType sceneType, Action onEnter)
    {
        OnEnterScene[sceneType] += onEnter;
    }

    public static void ListningExit(SceneType sceneType, Action onExit)
    {
        OnExitScene[sceneType] += onExit;
    }

    public static void StopListningEnter(SceneType sceneType, Action onEnter)
    {
        OnEnterScene[sceneType] -= onEnter;
    }

    public static void StopListningExit(SceneType sceneType, Action onExit)
    {
        OnExitScene[sceneType] -= onExit;
    }
}
