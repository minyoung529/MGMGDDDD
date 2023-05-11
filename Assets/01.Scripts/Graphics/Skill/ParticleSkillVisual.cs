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
    }
}
