using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePortal : ConnectionPortal
{
    public Action OnFire { get; set; }
    List<Func<int, bool>> canNotBurn = new List<Func<int, bool>>();
    private Fire fire;

    int key = 0;
    int fireKey = -1;

    private int index;

    private void OnTriggerStay(Collider other)
    {
        if (key == fireKey) return;

        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            fire ??= other.GetComponentInChildren<Fire>();

            if (fire.IsBurn && !CanNotBurn())
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

    public void StartListeningCanNotBurn(Func<int, bool> func)
    {
        canNotBurn.Add(func);
    }

    public bool CanNotBurn()
    {
        foreach (Func<int, bool> func in canNotBurn)
        {
            if (func.Invoke(index))
                return true;
        }

        return false;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
}
