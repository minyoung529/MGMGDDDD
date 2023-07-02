using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum AnimType
{
    Idle = 0,
    Afraid = 1,
    Follow = 2,
}
public class PlayPetAnimation : MonoBehaviour
{
    private Pet pet;
    private Animator anim;

    [SerializeField]
    private List<AnimationByPetEvent> animByPetEvents;

    private AnimType curType = AnimType.Idle;
    public AnimType CurAnim { get { return curType; } }
    private void Awake()
    {
        pet = GetComponent<Pet>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {

        foreach (AnimationByPetEvent animEvent in animByPetEvents)
        {
            animEvent.StartListening(() => ChangeAnimation(animEvent.animType));
            pet.Event.StartListening((int)animEvent.eventName, animEvent.Action);
        }
    }

    private void OnDestroy()
    {
        if (pet == null) return;
        if (pet.Event == null) return;

        foreach (AnimationByPetEvent animEvent in animByPetEvents)
        {
            pet.Event.StopListening((int)animEvent.eventName, animEvent.Action);
        }
    }

    public void ChangeAnimation(AnimType type, float delayTime)
    {
        if(curType == type) return;
        curType = type;
        anim.Play(type.ToString());
        StartCoroutine(CheckExitTime(delayTime));
    }

    public void ChangeAnimation(AnimType type)
    {
        curType = type;
        anim.Play(type.ToString());
    }

    private IEnumerator CheckExitTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime+0.1f);
        ChangeAnimation(AnimType.Idle);
    }
}

[System.Serializable]
public class AnimationByPetEvent
{
    public PetEventName eventName;
    public AnimType animType;

    private Action action;
    public Action Action => action;

    public void StartListening(Action action)
    {
        this.action += action;
    }

    public void StopListening(Action action)
    {
        this.action += action;
    }
}