using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePetEmission : MonoBehaviour
{
    private Renderer[] renderers;

    private readonly string EMISSION = "_Emission";
    private readonly string EMISSION_COLOR = "_EmissionColor";

    [SerializeField]
    private float intensity = 1.2f;

    private void Start()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
    }

    [ContextMenu("Change To White")]
    public void OnEmission()
    {
        ChangeEmission(true, Color.white * intensity);
    }

    public void OnEmission(Color color)
    {
        ChangeEmission(true, color);
    }

    [ContextMenu("Off Emission")]
    public void OffEmission()
    {
        ChangeEmission(false, Color.black);
    }

    private void ChangeEmission(bool isEmission, Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer.material.HasProperty(EMISSION))
            {
                renderer.material.SetInt(EMISSION, isEmission ? 1 : 0);
            }

            if (renderer.material.HasProperty(EMISSION_COLOR))
            {
                renderer.material.DOColor(color, EMISSION_COLOR, 1f);
            }
        }
    }
}
