using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationProperty : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string triggerName;

    public void Trigger()
    {
        Debug.Log("TRIGGER");
        animator.Play("Fall");
    }
}
