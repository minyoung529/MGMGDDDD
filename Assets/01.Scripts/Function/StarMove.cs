using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    Sequence sequence;

    [SerializeField]
    private float yDistance;

    [SerializeField]
    private float duration = 1f;

    private void Start()
    {
        sequence = DOTween.Sequence();

        Vector3 pos = transform.position;
        Vector3 destination = transform.position + Vector3.up * yDistance;

        //sequence.Append(transform.DOMove(destination, ));
    }

    private void OnDestroy()
    {
        sequence.Kill();
    }
}
