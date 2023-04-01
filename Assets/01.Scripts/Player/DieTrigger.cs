using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    public Action OnDie { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            PlayerRespawn.RespawnClosestPoint();
            OnDie?.Invoke();
        }
    }
}
