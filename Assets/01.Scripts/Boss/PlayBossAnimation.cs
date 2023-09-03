using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAnimType
{
    Idle = 0,
    Walk = 1,
    Run = 2,
    Catch = 3,
    Stun = 4
}
public class PlayBossAnimation : MonoBehaviour
{
    private Boss boss;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private List<AnimationByBossEvent> animByPetEvents;

    private BossAnimType curType = BossAnimType.Idle;
    public BossAnimType CurAnim { get { return curType; } }

    private void Awake()
    {
        boss = GetComponent<Boss>();
    }

    private void Start()
    {
        foreach (AnimationByBossEvent animEvent in animByPetEvents)
        {
            animEvent.StartListening(() => ChangeAnimation(animEvent.animType));
            boss.Event.StartListening((int)animEvent.eventName, animEvent.Action);
        }
    }

    private void OnDestroy()
    {
        if (boss == null) return;
        if (boss.Event == null) return;

        foreach (AnimationByBossEvent animEvent in animByPetEvents)
        {
            boss.Event.StopListening((int)animEvent.eventName, animEvent.Action);
        }
    }

    public void ChangeAnimation(BossAnimType type, float delayTime)
    {
        if (curType == type) return;
        curType = type;
        anim.Play(type.ToString());
        StartCoroutine(CheckExitTime(delayTime));
    }

    public void ChangeAnimation(BossAnimType type)
    {
        curType = type;
        anim.Play(type.ToString());
    }

    private IEnumerator CheckExitTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime + 0.1f);
        ChangeAnimation(BossAnimType.Idle);
    }

    public Animator GetAnimator() => anim;
}

[System.Serializable]
public class AnimationByBossEvent
{
    public BossEventName eventName;
    public BossAnimType animType;

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