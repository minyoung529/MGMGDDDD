using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class StopToSticky : MonoBehaviour
{
    [SerializeField] GearRotation[] gear;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.STICKY_PET_TAG))
        {
            StickyPet sticky = other.GetComponent<StickyPet>();
            sticky.SetAnimation(false);
            sticky.Skill();
            for(int i=0; i < gear.Length; i++)
            {
                gear[i].StopGear();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.STICKY_PET_TAG))
        {
            StickyPet sticky = other.GetComponent<StickyPet>();
            sticky.SetAnimation(true);
            sticky.NotSticky();
            for (int i = 0; i < gear.Length; i++)
            {
                gear[i].StartGear();
            }
        }
    }
}
