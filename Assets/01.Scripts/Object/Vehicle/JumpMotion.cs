using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpMotion
{
    public Vector3 targetPos;

    private Animator animator;

    private Vector3[] GetWayPoints(Transform player)
    {
        Vector3[] points = { player.position, Vector3.zero, targetPos };
        Vector3 dir = targetPos - player.position;
        points[1] = player.position + (dir * 0.5f) + Vector3.up * 3f;

        return points;
    }

    public void StartJump(Transform player, Action OnStartJump, Action OnEndJump, bool isSit = false)
    {
        OnStartJump?.Invoke();
        StartAnimation(player);

        player.DOPath(GetWayPoints(player), 1.5f, PathType.CatmullRom).OnComplete(() =>
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
        }).SetEase(Ease.InOutQuart);
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

        Debug.Log("LANDING");
        animator?.SetTrigger("tLanding");
    }
}
