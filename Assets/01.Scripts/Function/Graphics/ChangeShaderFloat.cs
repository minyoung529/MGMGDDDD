using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderFloat : MonoBehaviour
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float preDelay = 0f;
    private bool isActive;

    [SerializeField]
    new private string name;

    private int fillId;
    private Material material;

    [SerializeField]
    private Ease ease = Ease.OutQuad;

    [SerializeField]
    private float activeValue = 1f;

    [SerializeField]
    private float inactiveValue = 0f;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        fillId = Shader.PropertyToID(name);
    }

    public void Trigger()
    {
        if (isActive)
        {
            Inactive();
        }
        else
        {
            Active();
        }

        isActive = !isActive;
    }

    public void Active()
    {
        Sequence seq = DOTween.Sequence();

        if (preDelay != 0f)
        {
            seq.AppendInterval(preDelay);
        }

        seq.AppendCallback(() => ChangeValue(activeValue));
    }

    public void Inactive()
    {
        Sequence seq = DOTween.Sequence();

        if (preDelay != 0f)
        {
            seq.AppendInterval(preDelay);
        }
        seq.AppendCallback(() => ChangeValue(inactiveValue));
    }

    private void ChangeValue(float value)
    {
        material.DOKill();
        material.DOFloat(value, fillId, duration).SetEase(ease);
    }
}
