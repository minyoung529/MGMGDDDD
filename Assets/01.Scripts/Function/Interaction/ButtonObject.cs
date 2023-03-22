using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour, IFindable
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private UnityEvent OnButton;

    private bool isButtonOn = false;

    bool IFindable.IsFindable { get => true; }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            if (collision.transform.position.y > transform.position.y)
            {
                if (isButtonOn) return;

                OnButtonAnimation();
                OnButton?.Invoke();
                isButtonOn = true;
            }
        }
    }

    private void OnButtonAnimation()
    {
        transform.DOLocalMoveY(-0.31f, 0.7f);
    }
}
