using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePortal : ConnectionPortal
{
    public Action OnFire;
    private Fire fire;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            fire ??= other.GetComponent<Fire>();

            if (fire.IsBurn)
            {
                OnFire?.Invoke();
            }
        }
    }

    public void Listen(Action action)
    {
        OnFire += action;
    }
}
