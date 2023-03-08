using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    private PlayableDirector[] scenes = null;
    private Dictionary<string, PlayableDirector> sceneDictionary = new Dictionary<string, PlayableDirector>();

    private static Action OnCutsceneStart;
    private static Action OnCutsceneEnd;

    [SerializeField]
    protected Image topBar;
    [SerializeField]
    protected Image bottomBar;

    private void Awake()
    {
        scenes = GetComponentsInChildren<PlayableDirector>();
        for (int i = 0; i < scenes.Length; i++)
        {
            sceneDictionary.Add(scenes[i].name, scenes[i]);
            scenes[i].gameObject.SetActive(false);
        }
    }

    public void Play(string sceneName, float speed = 1f)
    {
        if (!sceneDictionary.ContainsKey(sceneName)) return;

        sceneDictionary[sceneName].gameObject.SetActive(true);
        sceneDictionary[sceneName].Play();
        sceneDictionary[sceneName].playableGraph.GetRootPlayable(0).SetSpeed(speed);
        OnCutsceneStart?.Invoke();

        StartCoroutine(WaitForDuration(sceneDictionary[sceneName]));
    }

    private IEnumerator WaitForDuration(PlayableDirector playableDirector)
    {
        ActiveBlackBar();

        yield return new WaitForSeconds((float)playableDirector.duration);
        playableDirector.gameObject.SetActive(false);
        OnCutsceneEnd?.Invoke();
        InactiveBlackBar();
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

    private void ActiveBlackBar()
    {
        bottomBar.rectTransform.DOAnchorPosY(150, 1f);
        topBar.rectTransform.DOAnchorPosY(-150, 1f);
    }

    private void InactiveBlackBar()
    {
        bottomBar.rectTransform.DOAnchorPosY(0, 1f);
        topBar.rectTransform.DOAnchorPosY(0, 1f);
    }
}
