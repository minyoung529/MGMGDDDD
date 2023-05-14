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
        Debug.Log("TRIGGER");
        animator.Play(triggerName);
    }

    public void ParticlePlay()
    {
        particle.gameObject.SetActive(true);
        particle.Play();
    }
}
