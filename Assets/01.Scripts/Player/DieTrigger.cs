using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    public Action OnDie { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            EventParam param = new();
            EventManager.TriggerEvent(EventName.PlayerDie, param);
            OnDie?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PLAYER_TAG))
        {
            EventParam param = new();
            EventManager.TriggerEvent(EventName.PlayerDie, param);
            OnDie?.Invoke();
        }
    }
}
