using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialCar : MonoBehaviour
{
    private void DeadPlayer()
    {
        Debug.Log("GameOver");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(Define.PLAYER_TAG))
        {
            DeadPlayer();
        }
    }
}
