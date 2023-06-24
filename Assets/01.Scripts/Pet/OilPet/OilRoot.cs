using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilRoot : MonoBehaviour
{
    public event EventHandler OnContactFirePet;

    private List<Fire> oilTriggerFires = new List<Fire>();

    private Fire firePet = null;

    public float BurnDuration => oilTriggerFires[0].BurningTime;

    public bool Complete { get; set; }

    public Action OnDryOil {get; set;}

    public Vector3[] Points
    {
        get
        {
            List<Vector3> list = new List<Vector3>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    list.Add(transform.GetChild(i).position);
            }

            return list.ToArray();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        DetectFire(other.gameObject, other.transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        DetectFire(other.gameObject, other.GetContact(0).point);
    }

    private void DetectFire(GameObject obj, Vector3 contactPoint)
    {
        if (obj.CompareTag(Define.FIRE_PET_TAG))
        {
            firePet ??= obj.GetComponent<Fire>();
            if (firePet == null) return;

            if (firePet.gameObject == obj.gameObject && firePet.IsBurn)
            {
                OnContactFirePet?.Invoke(GetNearestFire(contactPoint), EventArgs.Empty);
                Debug.Log("DETECT FIRE (PET)");
            }
        }
        else
        {
            Fire fire = obj.GetComponent<Fire>();

            if (fire != null && fire.IsBurn && fire.IsClingTo)
            {
                OnContactFirePet?.Invoke(GetNearestFire(contactPoint), EventArgs.Empty);
                Debug.Log("DETECT FIRE (NOT PET)");
            }
        }
    }

    private Fire GetNearestFire(Vector3 contactPosition)
    {
        float minDist = 999f;
        Fire fire = null;

        for (int i = 0; i < oilTriggerFires.Count; i++)
        {
            if (!oilTriggerFires[i].gameObject.activeSelf)
                continue;

            float dist = Vector3.Distance(oilTriggerFires[i].transform.position, contactPosition);

            if (minDist > dist)
            {
                minDist = dist;
                fire = oilTriggerFires[i];
            }
        }

        return fire;
    }

    public void ResetAllOil()
    {
        foreach (Fire fire in oilTriggerFires)
        {
            fire.StopBurn();
            fire.gameObject.SetActive(false);
        }
    }

    public void AddFire(Fire fire)
    {
        oilTriggerFires.Add(fire);
    }
}