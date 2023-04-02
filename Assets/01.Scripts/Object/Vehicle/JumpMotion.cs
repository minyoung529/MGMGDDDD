using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpMotion
{
    public Vector3 TargetPos { get; set; }

    private Animator animator;

    private Vector3[] GetWayPoints(Transform player)
    {
        Vector3[] points = { player.position, Vector3.zero, TargetPos };
        Vector3 dir = TargetPos - player.position;
        points[1] = player.position + (dir * 0.5f) + Vector3.up * 3f;

        return points;
    }

    public void StartJump(Transform player, Action OnStartJump, Action OnEndJump, bool isSit = false, float duration = 0.75f)
    {
        OnStartJump?.Invoke();
        StartAnimation(player);

        player.DOPath(GetWayPoints(player), duration, PathType.CatmullRom).OnComplete(() =>
        {
            if (isSit)
            {
                animator.SetTrigger("tStateChange");
                animator.SetInteger("iStateNum", (int)StateName.Sit);
            }
            else
            {
                EndAnimation(player);
            }
            OnEndJump?.Invoke();
        }).SetEase(Ease.InOutQuint);
    }

    public void Jump(Transform player, Transform target, float duration, UnityEvent endEvent = null)
    {
        TargetPos = target.position;

        player.DOKill();

        player.position = Vector3.zero;

        Debug.Log("jump");
        player.DOPath(GetWayPoints(player), duration, PathType.CatmullRom).OnComplete(() =>
        {
            endEvent?.Invoke();
        }).SetEase(Ease.InOutQuint);
    }

    private void StartAnimation(Transform player)
    {
        animator ??= player.GetComponent<Animator>();

        animator?.SetTrigger("tStateChange");
        animator?.SetInteger("iStateNum", (int)StateName.Jump);
    }

    private void EndAnimation(Transform player)
    {
        animator ??= player.GetComponent<Animator>();
        animator?.SetTrigger("tLanding");
    }
}
