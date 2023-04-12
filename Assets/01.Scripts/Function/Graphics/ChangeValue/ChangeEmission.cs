using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmission : MonoBehaviour
{
    [Header("Time")]
    [SerializeField]
    private bool isMaintain = true;

    [SerializeField]
    private float onTime = 1f;

    [SerializeField]
    private float offTime = 1f;

    [SerializeField]
    private float delay = 1f;

    [Header("Value")]
    [SerializeField]
    Color color;
    Color originalColor;
    [SerializeField]
    private float intensity = 1f;

    new private Renderer renderer;

    private readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalColor = GetEmission();
    }

    [ContextMenu("Change")]
    public void Change()
    {
        Color originalColor = GetEmission();
        Color emissionColor = color * intensity;

        renderer.material.DOColor(emissionColor, EMISSION_COLOR, onTime);

        if (!isMaintain)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(DOTween.To(GetEmission, SetEmission, originalColor, offTime));
        }
    }

    public void BackToOriginalColor()
    {
        renderer?.material.DOKill();
        renderer?.material.DOColor(originalColor, EMISSION_COLOR, onTime);
    }

    private Color GetEmission()
    {
        return renderer.material.GetColor(EMISSION_COLOR);
    }

    private void SetEmission(Color color)
    {
        renderer.material.SetColor(EMISSION_COLOR, color);
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public Color GetColor => color;

    public void SetIsMaintain(bool isMaintain)
    {
        this.isMaintain = isMaintain;
    }
}
