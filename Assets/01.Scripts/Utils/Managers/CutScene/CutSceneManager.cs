using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    private PlayableDirector[] scenes = null;
    private Dictionary<string, PlayableDirector> sceneDictionary = new Dictionary<string, PlayableDirector>();

    private static Action OnCutsceneStart;
    private static Action OnCutsceneEnd;

    private void Awake()
    {
        scenes = GetComponentsInChildren<PlayableDirector>();
        for (int i = 0; i < scenes.Length; i++)
        {
            sceneDictionary.Add(scenes[i].name, scenes[i]);
            scenes[i].gameObject.SetActive(false);
        }
    }

    public void Play(string sceneName)
    {
        sceneDictionary[sceneName].gameObject.SetActive(true);
        sceneDictionary[sceneName].Play();
        OnCutsceneStart?.Invoke();

        StartCoroutine(WaitForDuration(sceneDictionary[sceneName]));
    }

    private IEnumerator WaitForDuration(PlayableDirector playableDirector)
    {
        yield return new WaitForSeconds((float)playableDirector.duration);
        playableDirector.gameObject.SetActive(false);
        OnCutsceneEnd?.Invoke();
    }

    public static void AddStartCutscene(Action action)
    {
        OnCutsceneStart += action;
    }

    public static void RemoveStartCutscene(Action action)
    {
        OnCutsceneStart -= action;
    }

    public static void AddEndCutscene(Action action)
    {
        OnCutsceneEnd += action;
    }

    public static void RemoveEndCutscene(Action action)
    {
        OnCutsceneEnd -= action;
    }
}
