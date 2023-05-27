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

    public void Play(PlayableDirector director, float speed = 1f)
    {
        if(director == null) return;

        director.gameObject.SetActive(true);
        director.Play();
        director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        OnCutsceneStart?.Invoke();

        StartCoroutine(WaitForDuration(director, (float)director.duration * speed));
    }

    private IEnumerator WaitForDuration(PlayableDirector playableDirector, float duration)
    {
        ActiveBlackBar();

        yield return new WaitForSeconds(duration);
        OnCutsceneEnd?.Invoke();
        InactiveBlackBar();
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
}
