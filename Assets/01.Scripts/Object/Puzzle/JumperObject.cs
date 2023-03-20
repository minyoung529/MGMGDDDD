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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            playerRigid ??= collision.gameObject.GetComponent<Rigidbody>();
            playerOil ??= collision.gameObject.GetComponent<DetectOil>();

            playerRigid.velocity = Vector3.zero;
            playerRigid.angularVelocity = Vector3.zero;
            //playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

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
}
