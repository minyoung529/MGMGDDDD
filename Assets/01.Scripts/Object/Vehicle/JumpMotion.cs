using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpMotion
{
    public Vector3 TargetPos { get; set; }
    public float JumpHeight { get; set; } = 0.5f;

    private Animator animator;

    private Ease jumpEase = Ease.InOutSine;

    private Vector3[] GetWayPoints(Transform player)
    {
        Vector3[] points = { player.position, Vector3.zero, TargetPos };
        Vector3 dir = TargetPos - player.position;
        points[1] = player.position + (dir * JumpHeight) + Vector3.up * 3f;

        return points;
    }

    public void StartJump(Transform player, Action OnStartJump = null, Action OnEndJump = null, bool isSit = false, float duration = 0.75f)
    {
        OnStartJump?.Invoke();
        StartAnimation(player);

        player.DOPath(GetWayPoints(player), duration, PathType.CatmullRom).OnComplete(() =>
        {
            if (isSit)
            {
                GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.Sit);
            }
            else
            {
                EndAnimation(player);
            }
            OnEndJump?.Invoke();
        }).SetEase(jumpEase);
    }

    public void Jump(Transform player, Transform target, float duration, UnityEvent endEvent = null)
    {
        TargetPos = target.position;

        player.DOKill();

        player.DOPath(GetWayPoints(player), duration, PathType.CatmullRom).OnComplete(() =>
        {
            endEvent?.Invoke();
        }).SetEase(jumpEase);
    }

    public void Jump(Transform player, Transform target, float duration, Action action = null)
    {
        TargetPos = target.position;

        player.DOKill();

        //player.DOMove(target.position, duration);

        player.DOPath(GetWayPoints(player), duration, PathType.CatmullRom).OnComplete(() =>
        {
            action?.Invoke();
        }).SetEase(jumpEase);
    }


    private void StartAnimation(Transform player)
    {
        animator ??= player.GetComponent<Animator>();
        if (animator == null) return;

        GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.Jump);
    }

    private void EndAnimation(Transform player)
    {
        GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.DefaultMove);
        // animator ??= player.GetComponent<Animator>();
        // animator?.SetTrigger("tLanding");
    }

    public void SetEase(Ease ease)
    {
        jumpEase = ease;
    }
}
