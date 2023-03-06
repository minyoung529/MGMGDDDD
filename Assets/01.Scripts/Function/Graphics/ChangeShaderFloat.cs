using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Material material = null;

    [SerializeField]
    private Ease ease = Ease.OutQuad;

    [SerializeField]
    private float activeValue = 1f;

    [SerializeField]
    private float inactiveValue = 0f;

    private void Start()
    {
        if (GetComponent<Renderer>())
        {

            material = GetComponent<Renderer>().material;
        }
        material ??= GetComponent<RawImage>().material;
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

        Debug.Log(inactiveValue);
        seq.AppendCallback(() => ChangeValue(inactiveValue));
    }

    private void ChangeValue(float value)
    {
        material.DOKill();
        material.DOFloat(value, fillId, duration).SetEase(ease);
    }
}
