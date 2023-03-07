using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalngMalngTest : MonoBehaviour
{
    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleY(1.2f, 1f));
        seq.Append(transform.DOScaleY(1f, 1f));
        seq.SetLoops(-1);
    }
}
