using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ChangeLightValue : MonoBehaviour
{
    [SerializeField] private float activeIntensity = 1f;
    [SerializeField] private float inactiveIntensity = 0f;
    [SerializeField] private float duration = 0f;

    [SerializeField] private Ease ease;

    private Light light;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    public void Inactive()
    {
        light.DOIntensity(inactiveIntensity, duration).SetEase(ease);
    }
    
    public void Actove()
    {
        light.DOIntensity(activeIntensity, duration).SetEase(ease);
    }
}
