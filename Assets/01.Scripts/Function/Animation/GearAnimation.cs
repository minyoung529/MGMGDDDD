using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAnimation : MonoBehaviour
{
    private List<Animator> animators;

    private void Start()
    {
        animators = new List<Animator>(transform.GetComponentsInChildren<Animator>());
        Rotate();
    }

    public void Rotate()
    {
        foreach(Animator animator in animators)
        {
            animator.SetBool("Rotate", true);
        }
    }

    public void StopRotate()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Rotate", false);
        }
    }
}
