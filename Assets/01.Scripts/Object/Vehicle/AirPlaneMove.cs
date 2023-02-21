using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public enum PlaneState
{
    Idle, Move, Count
}
public class AirPlaneMove : MonoBehaviour
{
    private PlaneState planeState;
    [Header("Idle")]
    [SerializeField]
    private float duration;
    [SerializeField]
    private float distance = 1f;
    private Sequence idleSequence;

    [Header("Move")]
    [SerializeField]
    private float moveDistance = 14f;
    [SerializeField]
    private bool isDown = false;
    [SerializeField]
    private Transform[] targetPath;

    [SerializeField]
    private UnityEvent OnArrive;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        if (!isDown)
        {
            StartIdleMove();
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void StartIdleMove()
    {
        idleSequence.Kill();

        float originalY = transform.position.y;
        idleSequence = DOTween.Sequence();
        idleSequence.Append(transform.DOMoveY(originalY + distance, duration).SetEase(Ease.OutQuad));
        idleSequence.Append(transform.DOMoveY(originalY, duration).SetEase(Ease.OutQuad));
        idleSequence.SetLoops(-1);
    }

    public void MoveY()
    {
        moveDistance = (isDown) ? Mathf.Abs(moveDistance) : -Mathf.Abs(moveDistance);

        idleSequence.Kill();

        if (targetPath.Length > 0)
        {
            Vector3[] wayPoints = new Vector3[targetPath.Length];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = targetPath[i].position;
                UnityEngine.Debug.Log(wayPoints[i]);
            }

            if (isDown)
            {
                transform.DOPath(wayPoints, 5f, PathType.Linear).OnComplete(() => OnArrive.Invoke());
                transform.DORotate(targetPath[^1].eulerAngles, 5f);
            }
            else
            {
                transform.DOMove(originalPosition, 5f).OnComplete(() => OnArrive.Invoke());
                transform.DORotateQuaternion(originalRotation, 5f);
            }
        }
        else
        {
            transform.DOMoveY(transform.position.y + moveDistance, 5f).OnComplete(() => OnArrive.Invoke());
        }

        isDown = !isDown;
    }
}
