using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    private Sticky sticky;
    private JumpMotion jumpMotion;

    private bool isInserted = false;

    private Action onInserted;

    [SerializeField]
    private float distance = 5f;

    [SerializeField]
    private float jumpHeight = 0.5f;

    private void Awake()
    {
        sticky = GetComponent<Sticky>();
        jumpMotion = new JumpMotion();
    }

    void Update()
    {
        if (isInserted) return;

        if (Vector3.Distance(targetTransform.position, transform.position) < distance)
        {
            isInserted = true;
            OnInserted();
        }
    }

    private void OnInserted()
    {
        sticky?.NotSticky();

        jumpMotion.TargetPos = targetTransform.position;
        jumpMotion.JumpHeight = jumpHeight;
        jumpMotion.Jump(transform, targetTransform, 0.7f, onInserted);
    }

    public void ListeningOnInserted(Action onInsertBolt)
    {
        onInserted += onInsertBolt;
    }
}
