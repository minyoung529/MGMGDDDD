using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePortal : ConnectionPortal
{
    public Action OnFire { get; set; }
    private Func<bool> CanBurn;
    private Fire fire;

    int key = 0;
    int fireKey = -1;

    private void OnTriggerStay(Collider other)
    {
        if (key == fireKey) return;

        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            fire ??= other.GetComponent<Fire>();

            if (fire.IsBurn && !CanBurn.Invoke())
            {
                fireKey = key;
                OnFire?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            key++;
        }
    }

    public void StartListeningBurn(Action action)
    {
        OnFire += action;
    }

    public void StartListeningCanBurn(Func<bool> func)
    {
        CanBurn += func;
    }
}
