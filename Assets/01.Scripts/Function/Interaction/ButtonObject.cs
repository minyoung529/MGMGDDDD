using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private UnityEvent OnButton;

    private bool isButtonOn = false;

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
        transform.DOLocalMoveY(-0.37f, 0.7f);
    }
}
