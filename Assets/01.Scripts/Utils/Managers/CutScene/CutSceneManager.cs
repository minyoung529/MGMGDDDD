using System.ComponentModel;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutSceneManager : MonoSingleton<CutSceneManager>
{
    private Action OnCutsceneStart;
    private Action OnCutsceneEnd;

    [SerializeField]
    protected Image topBar;
    [SerializeField]
    protected Image bottomBar;

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    public void Play(PlayableDirector director, Action callback)
    {
        if(director == null) return;

        director.gameObject.SetActive(true);
        director.Play();
        director.playableGraph.GetRootPlayable(0).SetSpeed(1f);
        OnCutsceneStart?.Invoke();
        isPlaying = true;

        StartCoroutine(WaitForDuration(director, (float)director.duration * 1f, callback));
    }

    private IEnumerator WaitForDuration(PlayableDirector playableDirector, float duration, Action callback)
    {
        ActiveBlackBar();

        yield return new WaitForSeconds(duration);
        OnCutsceneEnd?.Invoke();
        callback?.Invoke();
        InactiveBlackBar();
        isPlaying = false;
    }

    public void AddStartCutscene(Action action)
    {
        OnCutsceneStart += action;
    }

    public void RemoveStartCutscene(Action action)
    {
        OnCutsceneStart -= action;
    }

    public void AddEndCutscene(Action action)
    {
        OnCutsceneEnd += action;
    }

    public void RemoveEndCutscene(Action action)
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

    public void EnterCutscene()
    {
        OnCutsceneStart?.Invoke();
    }

    public void ExitCutscene()
    {
        OnCutsceneEnd?.Invoke();
    }
}
