using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CallJumpMotion : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private UnityEvent endEvent = null;

    private JumpMotion jumpMotion = new();

    [SerializeField]
    private Ease ease = Ease.InOutSine;

    [ContextMenu("PLAYER")]
    public void JumpToTarget()
    {
        jumpMotion.SetEase(ease);
        jumpMotion.Jump(player, target, duration, endEvent);
    }
}
