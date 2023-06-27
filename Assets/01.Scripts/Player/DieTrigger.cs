using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class DieTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onDieEvent;
    public Action OnDie { get; set; }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.PLAYER_TAG))
        {
            EventParam param = new();
            EventManager.TriggerEvent(EventName.PlayerDie, param);
            onDieEvent?.Invoke();
            OnDie?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PLAYER_TAG))
        {
            EventParam param = new();
            EventManager.TriggerEvent(EventName.PlayerDie, param);
            onDieEvent?.Invoke();
            OnDie?.Invoke();
        }
    }
}
