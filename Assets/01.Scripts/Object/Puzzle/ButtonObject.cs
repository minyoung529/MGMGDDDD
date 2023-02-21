using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPushButton;
    [SerializeField] private UnityEvent OnExitButton;

    [SerializeField] private LayerMask playerLayer;

    private bool isPushed = false;
    private bool isMoving = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isPushed || isMoving) return;

        if (((1 << collision.gameObject.layer) & (playerLayer)) != 0)
        {
            if (transform.position.y < collision.transform.position.y)
            {
                Debug.Log("PUSH");
                isPushed = isMoving = true;
                OnPushButton?.Invoke();
                PushButtonMove();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isPushed || isMoving) return;

        if (((1 << collision.gameObject.layer) & (playerLayer)) != 0)
        {
            Debug.Log("EXIT");
            isPushed = isMoving = false;
            OnExitButton?.Invoke();
            ExitButtonMove();
        }
    }

    private void PushButtonMove()
    {
        transform.DOLocalMoveY(-0.25f, 0.5f).OnComplete(() => isMoving = false);
    }

    private void ExitButtonMove()
    {
        transform.DOLocalMoveY(0f, 0.5f).OnComplete(() => isMoving = false);
    }
}
