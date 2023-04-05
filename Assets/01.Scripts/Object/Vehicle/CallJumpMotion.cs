using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [ContextMenu("PLAYER")]
    public void JumpToTarget()
    {
        jumpMotion.Jump(player, target, duration, endEvent);
    }
}
