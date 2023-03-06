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
    [SerializeField]
    private float intensity = 1f;

    new private Renderer renderer;

    private readonly int hash = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    [ContextMenu("Change")]
    public void Change()
    {
        Color originalColor = GetEmission();
        Color emissionColor = color * intensity;
        Sequence seq = DOTween.Sequence();

        seq.Append(DOTween.To(GetEmission, SetEmission, emissionColor, onTime));

        if (!isMaintain)
        {
            seq.AppendInterval(delay);
            seq.Append(DOTween.To(GetEmission, SetEmission, originalColor, offTime));
        }
    }

    private Color GetEmission()
    {
        return renderer.material.GetColor(hash);
    }

    private void SetEmission(Color color)
    {
        renderer.material.SetColor(hash, color);
    }
}
