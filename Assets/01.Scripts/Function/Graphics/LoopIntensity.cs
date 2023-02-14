using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopIntensity : MonoBehaviour
{
    [SerializeField] private float startValue, endValue;
    [SerializeField] private float duration;
    new Light light;

    void Start()
    {
        light = GetComponent<Light>();

        Sequence seq = DOTween.Sequence();
        seq.Append(light.DOIntensity(endValue, duration));
        seq.Append(light.DOIntensity(startValue, duration));
        seq.SetLoops(-1);
    }
}
