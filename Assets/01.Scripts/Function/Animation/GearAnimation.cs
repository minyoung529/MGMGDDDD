using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearAnimation : MonoBehaviour
{
    [SerializeField] private UnityEvent rotateEvent;
    [SerializeField] private UnityEvent stopRotateEvent;

    private Animator[] animators;

    private void Start()
    {
        animators = transform.GetComponentsInChildren<Animator>();
    }

    [ContextMenu("Roll")]
    public void Rotate()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Rotate", true);
        }

        rotateEvent?.Invoke();
    }

    public void StopRotate()
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("Rotate", false);
        }
        stopRotateEvent?.Invoke();
    }
}
