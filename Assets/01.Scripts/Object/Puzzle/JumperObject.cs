using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumperObject : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 100f;

    [SerializeField]
    private float oilWeight = 2f;

    private Rigidbody playerRigid;
    private DetectOil playerOil;

    [SerializeField]
    private UnityEvent OnJump;

    [SerializeField]
    private Transform animationObject;

    public bool CanJump { get; set; } = true;

    private bool isAnimation = false;

    [SerializeField]
    private LayerMask layerMask;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!CanJump) return;

    //    if (collision.transform.CompareTag(Define.PLAYER_TAG))
    //    {
    //        playerRigid ??= collision.gameObject.GetComponent<Rigidbody>();
    //        playerOil ??= collision.gameObject.GetComponent<DetectOil>();

    //        playerRigid.velocity = Vector3.zero;
    //        playerRigid.angularVelocity = Vector3.zero;
    //        //playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    //        Jump();

    //        if (!isAnimation)
    //            JumpAnimation();
    //    }
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (!CanJump) return;

        if (collider.CompareTag(Define.PLAYER_TAG) && ((1 << collider.gameObject.layer) & layerMask) != 0)
        {
            playerRigid ??= collider.gameObject.GetComponent<Rigidbody>();
            playerOil ??= collider.gameObject.GetComponent<DetectOil>();
            playerOil ??= collider.transform.parent.GetComponentInChildren<DetectOil>();

            playerRigid.velocity = Vector3.zero;
            playerRigid.angularVelocity = Vector3.zero;
            //playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Jump();

            if (!isAnimation)
                JumpAnimation();
        }
    }

    private void Jump()
    {
        if (playerOil.IsContactOil)
        {
            playerRigid.velocity = Vector3.up * jumpForce * oilWeight;
        }
        else
        {
            playerRigid.velocity = Vector3.up * jumpForce;
        }

        OnJump?.Invoke();
    }

    private void JumpAnimation()
    {
        isAnimation = true;

        Sequence seq = DOTween.Sequence();

        Vector3 originalScale = animationObject.localScale;

        seq.Append(animationObject.DOScale(originalScale * 0.85f, 0.25f).SetEase(Ease.OutBounce));
        seq.Append(animationObject.DOScale(originalScale, 0.2f));
        seq.AppendCallback(() => isAnimation = false);
    }
}
