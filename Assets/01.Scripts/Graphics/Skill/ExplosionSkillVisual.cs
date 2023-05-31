using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkillVisual : SkillVisual
{
    [SerializeField]
    private Transform changedObject;

    [SerializeField]
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    private Vector3 originalScale = Vector3.one;

    private void Start()
    {
        originalScale = changedObject.localScale;

        foreach (ParticleSystem particle in particles)
        {
            particle.transform.SetParent(changedObject.transform);
        }
    }

    protected override void OnTrigger()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(changedObject.DOScale(originalScale * 6f, 1.5f).SetEase(Ease.InBounce));

        seq.AppendCallback(() =>
        {
            changedObject.DOScale(originalScale * 0.45f, 0.2f);
            particles.ForEach(x => x.Play());
        });

        seq.AppendInterval(3.2f);
        seq.Append(changedObject.DOScale(originalScale, 1f));
        seq.AppendCallback(() => onComplete?.Invoke());
    }
}
