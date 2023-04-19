using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyJumpToTarget : MonoBehaviour
{
    private StickyPet stickyPet = null;

    [SerializeField]
    private Transform target;
    private JumpMotion jumpMotion = new JumpMotion();

    private void Awake()
    {
        jumpMotion.TargetPos = target.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("StickyPet"))
        {
            stickyPet ??= collision.gameObject.GetComponent<StickyPet>();

            if (stickyPet == null || stickyPet.State != StickyState.Billow) return;

            Jump();
        }
    }

    private void Jump()
    {
        jumpMotion.StartJump(transform);
    }
}
