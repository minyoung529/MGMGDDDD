using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWalkAnimation : MonoBehaviour
{
    private Vector3 prevPosition = Vector3.one;
    private Animator animator;

    void Start()
    {
        prevPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ((prevPosition - transform.position).magnitude > 0.05f)
        {
            animator?.SetBool("Walk", true);
        }
        else
        {
            animator?.SetBool("Walk", false);
        }

        prevPosition = transform.position;
    }
}
