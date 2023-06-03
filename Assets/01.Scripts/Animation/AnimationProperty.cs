using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationProperty : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string triggerName;

    [SerializeField]
    private ParticleSystem particle;

    public void Trigger()
    {
        animator.Play(triggerName);
    }

    public void ParticlePlay()
    {
        if(particle)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }
    }
}
