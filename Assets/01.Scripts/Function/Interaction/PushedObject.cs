using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushedObject : MonoBehaviour
{
    private bool isContactPlayer = false;
    private bool isMove = false;

    [SerializeField]
    private UnityEvent onPushStart;

    [SerializeField]
    private UnityEvent onPushEnd;

    private Vector3 prevPosition;

    void Start()
    {
        prevPosition = transform.position;
    }

    private void Update()
    {
        bool prevMove = isMove;
        isMove = ((prevPosition - transform.position).sqrMagnitude > 0.001f);
        prevPosition = transform.position;
        
        if (isContactPlayer)
        {
            if (prevMove != isMove)
            {
                if (isMove)
                {
                    onPushStart?.Invoke();
                }
                else
                {
                    onPushEnd?.Invoke();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            isContactPlayer = true;

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            isContactPlayer = false;
            onPushEnd?.Invoke();
        }
    }
}
