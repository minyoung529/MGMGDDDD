using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class AirPlaneMove : MonoBehaviour
{
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
    private bool isPropellerAllPause = false;
    [SerializeField]
    private Transform[] targetPath;

    [SerializeField]
    private UnityEvent<bool> OnArrive;

    [SerializeField]
    private UnityEvent<bool> OnDepart;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public bool isActive = false;

    private void Awake()
    {
        if (!isDown)
        {
            StartIdleMove();
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        gameObject.SetActive(isActive);
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

        bool tempDown = isDown;
        isDown = !isDown;

        bool down = isDown;
        if (isPropellerAllPause)
            down = true;

        OnDepart?.Invoke(isDown);

        if (targetPath.Length > 0)
        {
            Vector3[] wayPoints = new Vector3[targetPath.Length];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = targetPath[i].position;
            }

            if (tempDown)
            {
                transform.DOPath(wayPoints, 5f, PathType.CatmullRom).OnComplete(() => OnArrive.Invoke(down));
                transform.DORotate(targetPath[^1].eulerAngles, 5f);
            }
            else
            {
                transform.DOMove(originalPosition, 5f).OnComplete(() => OnArrive.Invoke(down));
                transform.DORotateQuaternion(originalRotation, 5f);
            }
        }
        else
        {
            transform.DOMoveY(transform.position.y + moveDistance, 5f).OnComplete(() => OnArrive.Invoke(down));
        }
    }
}
