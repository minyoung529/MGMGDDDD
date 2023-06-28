using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class OilTrigger : MonoBehaviour
{
    [SerializeField]
    private DecalProjector projector;

    [SerializeField]
    private float alpha = 1.0f;
    private readonly int ALPHA_HASH = Shader.PropertyToID("_Alpha");

    private void Awake()
    {
        projector.material = new Material(projector.material);
    }

    private void OnEnable()
    {
        projector.material.SetFloat(ALPHA_HASH, 0f);
        projector.material.DOFloat(alpha, ALPHA_HASH, 0.3f);
    }

    private void OnDisable()
    {
        projector.material.SetFloat(ALPHA_HASH, 0f);
    }
}
