using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Bloom = UnityEngine.Rendering.Universal.Bloom;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Events;

public class ChangeBloom : MonoBehaviour
{
    [SerializeField]
    private Volume volume;

    private Bloom bloom;

    [Header("Value")]
    [SerializeField]
    private float threshold;
    private float originalThreshold;

    [SerializeField]
    private float intensity;
    private float originalIntensity;

    [SerializeField]
    private Color tintColor;
    private Color originalTintColor;

    [SerializeField]
    private float duration = 1f;

    [Header("IsChange")]
    [SerializeField]
    private bool[] isChange = new bool[3];

    [SerializeField]
    private UnityEvent onChange;

    private void Awake()
    {
        volume ??= FindObjectOfType<Volume>();

        volume.profile.TryGet(out bloom);
        originalIntensity = bloom.intensity.value;

        if (volume == null)
            Destroy(gameObject);
    }

    public void Change()
    {
        if (isChange[0])
        {
            DOTween.To
            (
                () => bloom.threshold.value,
                value => bloom.threshold.Override(value),
                threshold,
                duration
            );
        }
        if (isChange[1])
        {
            DOTween.To
            (
                () => bloom.intensity.value,
                value => bloom.intensity.Override(value),
                intensity,
                duration
            );
        }
        if(isChange[2])
        {
            DOTween.To
            (
                () => bloom.tint.value,
                value => bloom.tint.Override(value),
                tintColor,
                duration
            );
        }

        onChange?.Invoke();
    }
}
