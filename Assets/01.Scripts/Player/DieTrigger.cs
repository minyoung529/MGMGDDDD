using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            Debug.Log("DIE");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PLAYER_TAG))
        {
            Debug.Log("DIE");
        }
    }
}
