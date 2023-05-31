using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSkillVisual : SkillVisual
{
    [SerializeField]
    protected List<ParticleSystem> particles;

    protected override void OnTrigger()
    {
        particles.ForEach(x => x.Play());

        float maxDuration = 0f;

        for (int i = 0; i < particles.Count; i++)
        {
            maxDuration = Mathf.Max(maxDuration, particles[i].main.duration);
        }

        StartCoroutine(Delay(maxDuration));
    }

    private IEnumerator Delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }
}
