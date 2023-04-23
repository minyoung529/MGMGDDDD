using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    Move, Interaction, PetBasic, PetSelect
}

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private Canvas tutorialCanvasPrefab;
    private Canvas tutorialCanvas;

    Dictionary<TutorialType, TutorialPlayer> tutorials = new Dictionary<TutorialType, TutorialPlayer>();

    private Animator animator;

    private Action onEnd;

    private void Awake()
    {
        GameObject tutorial = GameObject.Find("TutorialCanvas");

        if (tutorial)
        {
            tutorialCanvas = tutorial.GetComponent<Canvas>();
        }
        else
        {
            tutorialCanvas = Instantiate(tutorialCanvasPrefab);
            tutorialCanvas.name = "TutorialCanvas";
        }
        animator = GetComponent<Animator>();

        TutorialPlayer[] players = tutorialCanvas.GetComponentsInChildren<TutorialPlayer>();

        foreach (TutorialPlayer player in players)
        {
            tutorials.Add(player.TutorialType, player);
            tutorials[player.TutorialType].Animator = animator;
            player.Animator = animator;
            player.Init();
        }
    }

    public void StartTutorial(TutorialType type, Action onEnd, string name = "")
    {
        tutorials[type].Animator = animator;
        tutorials[type].StartTutorial(name);
        this.onEnd += onEnd;
    }

    public void StopTutorial(TutorialType type)
    {
        tutorials[type].Animator = animator;
        tutorials[type].StopTutorial();

        onEnd?.Invoke();
        onEnd = null;
    }
}
