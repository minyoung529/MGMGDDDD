using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PaintedOil : MonoBehaviour
{
    public event EventHandler OnContactFirePet;

    private Fire fire = null;

    private Fire firePet = null;

    private void Awake()
    {
        fire = GetComponent<Fire>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.FIRE_PET_TAG))
        {
            firePet ??= other.GetComponent<Fire>();

            if (firePet.gameObject == other.gameObject && firePet.IsBurn)
            {
                OnContactFirePet?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Fire fire = other.GetComponent<Fire>();
                if(fire.IsBurn)
                {
                    OnContactFirePet?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public void Burn()
    {
        fire.Burn();
    }

    public void ResetOil()
    {
        fire.StopBurn();
        gameObject.SetActive(false);
    }
}