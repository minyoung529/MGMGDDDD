using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float preDelay = 0f;
    private bool isFill;

    private readonly int FILL_ID = Shader.PropertyToID("_Fill");
    private Material material;

    [SerializeField]
    private Ease ease = Ease.OutQuad;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void Trigger()
    {
        if (isFill)
        {
            UnFilling();
        }
        else
        {
            Filling();
        }

        isFill = !isFill;
    }

    public void Filling()
    {
        Sequence seq = DOTween.Sequence();

        if (preDelay != 0f)
        {
            seq.AppendInterval(preDelay);
        }

        seq.AppendCallback(() => ChangeValue(1f));
    }

    public void UnFilling()
    {
        Sequence seq = DOTween.Sequence();

        if (preDelay != 0f)
        {
            seq.AppendInterval(preDelay);
        }
        seq.AppendCallback(() => ChangeValue(0f));
    }

    private void ChangeValue(float value)
    {
        material.DOKill();
        material.DOFloat(value, FILL_ID, duration).SetEase(ease);
    }
}
