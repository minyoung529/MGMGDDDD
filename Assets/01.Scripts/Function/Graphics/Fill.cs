using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private bool isFill;

    private readonly int FILL_ID = Shader.PropertyToID("_Fill");
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void Trigger()
    {
        if(isFill)
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
        ChangeValue(1f);
    }

    public void UnFilling()
    {
        ChangeValue(0f);
    }

    private void ChangeValue(float value)
    {
        material.DOKill();
        material.DOFloat(value, FILL_ID, duration).SetEase(Ease.OutQuad);
    }
}
