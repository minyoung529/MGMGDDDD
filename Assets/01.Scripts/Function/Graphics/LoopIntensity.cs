using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopIntensity : MonoBehaviour
{
    [SerializeField] private float startValue, endValue;
    [SerializeField] private float duration;
    new Light light;

    [SerializeField]
    private bool gameStart = true;

    void Start()
    {
        light = GetComponent<Light>();

        if (gameStart)
            Loop();
    }

    private void Loop()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(light.DOIntensity(endValue, duration));
        seq.Append(light.DOIntensity(startValue, duration));
        seq.SetLoops(-1);
    }

    public void StartLighting()
    {
        light.intensity = 0;
        light.DOIntensity(startValue, 1f).OnComplete(Loop);
    }
}
