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
        seq.AppendCallback(ActiveIntensity);
        seq.AppendInterval(RandomDuration());
        seq.AppendCallback(InactiveIntensity);
        seq.AppendInterval(RandomDuration());
        seq.SetLoops(-1);
    }

    public void StartLighting()
    {
        light.intensity = 0;
        light.DOIntensity(startValue, 1f).OnComplete(Loop);
    }

    private void ActiveIntensity()
    {
        light.DOIntensity(endValue, duration);
    }

    private void InactiveIntensity()
    {
        light.DOIntensity(startValue, duration);
    }

    private float RandomDuration()
    {
        return Mathf.Clamp(Random.Range(duration - 0.5f, duration + 0.5f), 0f, duration);
    }
}
