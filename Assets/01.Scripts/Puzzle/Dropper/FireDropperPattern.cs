using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDropperPattern : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] fireParticle;

    private MeshRenderer[] meshRenderers;
    private Collider[] colliders;

    private void Awake()
    {
        gameObject.SetActive(true);
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        gameObject.SetActive(false);
    }
    public void StartDropper()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.95f);
        seq.AppendCallback(PlayParticlesAtAll);
        seq.AppendInterval(0.25f);
        seq.AppendCallback(() =>
        {
            foreach (MeshRenderer renderer in meshRenderers)
                renderer.enabled = false;

            foreach (Collider col in colliders)
                col.enabled = false;
        });
    }

    [ContextMenu("Play Particle")]
    public void PlayParticlesAtAll()
    {
        foreach (var particle in fireParticle)
        {
            particle.Play();
        }
    }
}
