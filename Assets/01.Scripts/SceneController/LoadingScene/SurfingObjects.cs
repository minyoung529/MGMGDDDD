using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfingObjects : MonoBehaviour
{
    [SerializeField]
    private Transform[] objects;

    [SerializeField]
    private float yDistance = 10f;

    [SerializeField] private float moveInterval = 1f;
    [SerializeField] private float moveDuration = 1f;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < objects.Length; i++)
        {
            Transform trn = objects[i];
            seq.AppendCallback(() => MoveObject(trn));
            seq.AppendInterval(moveInterval);
        }
    }

    private void MoveObject(Transform transform)
    {
        Sequence seq = DOTween.Sequence();
        float yPos = transform.position.y;

        seq.Append(transform.DOMoveY(yPos + yDistance, moveDuration * 0.5f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOMoveY(yPos, moveDuration * 0.5f).SetEase(Ease.InQuad));
        seq.SetLoops(-1);
    }
}
