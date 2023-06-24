using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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

    [SerializeField]
    private bool isStartIdle = true;

    private float positionY = 0f;
    private float percentage = 0f;

    [SerializeField]
    private float wayPointDuration = 10f;

    [SerializeField]
    private GameObject tutorialTrigger;

    private void Awake()
    {
        if (!isDown && isStartIdle)
        {
            StartIdleMove();
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        gameObject.SetActive(isActive);
    }

    public void StartIdleMove()
    {
        idleSequence.Kill();

        positionY = transform.position.y;
        idleSequence = DOTween.Sequence();
        idleSequence.Append(transform.DOMoveY(positionY + distance, duration).SetEase(Ease.OutQuad));
        idleSequence.Append(transform.DOMoveY(positionY, duration).SetEase(Ease.OutQuad));
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
            transform.DOKill();

            Vector3[] wayPoints = new Vector3[targetPath.Length];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = targetPath[i].position;
            }

            if (tempDown)
            {
                transform.DOPath(wayPoints, wayPointDuration, PathType.CatmullRom)
                .OnComplete(() => Arrive(down))
                .SetEase(Ease.InOutQuad);

                transform.DOLookAt(wayPoints[wayPoints.Length - 1], wayPointDuration);
            }
            else
            {
                transform.DOMove(originalPosition, wayPointDuration)
                .OnComplete(() => Arrive(down));
            }
        }
        else
        {
            transform.DOMoveY(transform.position.y + moveDistance, 5f)
            .OnComplete(() => Arrive(down));
        }
    }

    private void Arrive(bool down)
    {
        OnArrive.Invoke(down);
    }

    private void OnDestroy()
    {
        idleSequence.Kill();
        transform.DOKill();
    }
}
