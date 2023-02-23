using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperObject : MonoBehaviour
{
    [SerializeField]
    private  float jumpForce = 100f;
    private Rigidbody playerRigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            playerRigid ??= collision.gameObject.GetComponent<Rigidbody>();
            
            playerRigid.velocity = Vector3.zero;
            playerRigid.angularVelocity = Vector3.zero;
            //playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            playerRigid.velocity = Vector3.up * jumpForce;
        }
    }
}
