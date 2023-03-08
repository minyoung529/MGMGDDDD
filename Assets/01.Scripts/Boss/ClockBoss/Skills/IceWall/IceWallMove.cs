using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceWallMove : MonoBehaviour
{
    private Sequence spawnSeq;
    private Sequence destroySeq;
    private float spawnTime;

    public void Move(Vector3 size, float spawnTime, float distance, float speed) {
        this.spawnTime = spawnTime;
        spawnSeq = DOTween.Sequence();
        spawnSeq.Append(transform.DOScale(size, spawnTime));
        spawnSeq.Append(transform.DOMove(transform.position + transform.forward * distance, distance / speed));
        spawnSeq.AppendCallback(() => Destroy());
    }

    public void Destroy() {
        spawnSeq.Kill();
        destroySeq = DOTween.Sequence();
        destroySeq.Append(transform.DOScale(Vector3.zero, spawnTime));
        destroySeq.AppendCallback(() => Destroy(gameObject));
    }

    private void OnDestroy() {
        destroySeq.Kill();
    }
}
