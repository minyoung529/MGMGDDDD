using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeStepEmission : MonoBehaviour
{
    [Header("Render")]
    [SerializeField]
    private float interval = 1f;

    [SerializeField]
    private Transform[] rendererParents;

    private List<List<Renderer>> renderers = new List<List<Renderer>>();

    private int changeCount = 0;
    private readonly int hash = Shader.PropertyToID("_EmissionColor");

    [SerializeField]
    private UnityEvent SuccessAllChange;

    [Header("Color")]
    [SerializeField]
    private Color color;
    [SerializeField]
    private float intensity;

    [SerializeField]
    private Color originalColor;
    [SerializeField]
    private float originalIntensity;

    void Start()
    {
        for (int i = 0; i < rendererParents.Length; i++)
        {
            Renderer[] childs = rendererParents[i].GetComponentsInChildren<Renderer>();
            renderers.Add(new List<Renderer>());

            foreach (Renderer renderer in childs)
            {
                renderers[i].Add(renderer);
            }
        }

        BackToOriginalColor();
    }

    public void Trigger()
    {
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < changeCount; i++)
        {
            int index = i;
            seq.AppendCallback(() => ChangeRenderers(index, color));
            seq.AppendInterval(interval);
        }

        seq.AppendCallback(After);
    }

    private void ChangeRenderers(int index, Color color)
    {
        foreach (Renderer renderer in renderers[index])
        {
            renderer.material.SetColor(hash, color * intensity);
        }
    }

    private void BackToOriginalColor()
    {
        for (int i = 0; i < rendererParents.Length; i++)
        {
            ChangeRenderers(i, originalColor);
        }
    }

    public void SetChangeCount(int changeCount)
    {
        this.changeCount = changeCount;
    }

    public void SetAndTrigger(int changeCount)
    {
        SetChangeCount(changeCount);
        Trigger();
    }

    private void After()
    {
        if (changeCount == rendererParents.Length)
        {
            SuccessAllChange?.Invoke();
        }
        else
        {
            BackToOriginalColor();
        }
    }

    public void DelaySetChangeCount(int changeCount)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(2.5f);
        seq.AppendCallback(() => SetChangeCount(changeCount));
    }
}
