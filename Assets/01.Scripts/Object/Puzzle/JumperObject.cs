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

    public bool CanJump { get; set; } = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!CanJump) return;

        if (collision.transform.CompareTag(Define.PLAYER_TAG))
        {
            playerRigid ??= collision.gameObject.GetComponent<Rigidbody>();
            playerOil ??= collision.gameObject.GetComponent<DetectOil>();

            playerRigid.velocity = Vector3.zero;
            playerRigid.angularVelocity = Vector3.zero;
            //playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Jump();
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
}
