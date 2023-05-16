using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SameTimeTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onClear;

    [SerializeField] GameObject[] holograms;
    private Dictionary<PetType, Pet> triggerPets;

    private bool playing = false;
    Action clearTriggerAction;

    private void Awake()
    {
        triggerPets = new Dictionary<PetType, Pet>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Define.PET_LAYER)
        {
            Pet pet = other.GetComponent<Pet>();
            if (pet == null) return;

            if (triggerPets.ContainsKey(pet.GetPetType) == false)
            {
                InnerPet(pet);
            }

        }
    }


    private void InnerPet(Pet p)
    {
        triggerPets.Add(p.GetPetType, p);
        p.SetTarget(holograms[((int)p.GetPetType)-1].transform);

        clearTriggerAction = ()=>ClearTrigger(p);
        p.Event.StartListening((int)PetEventName.OnArrive, clearTriggerAction);
    }
    private void ClearTrigger(Pet p)
    {
        Debug.Log("Clear");
        holograms[((int)p.GetPetType) - 1].gameObject.SetActive(false);

        p.Event.StopListening((int)PetEventName.OnArrive, clearTriggerAction);
        clearTriggerAction = null;
        p.SetTarget(null);

        if (triggerPets.Count >= 3)
        {
            triggerPets[PetType.FirePet].gameObject.SetActive(false);
            triggerPets[PetType.StickyPet].gameObject.SetActive(false);
            triggerPets[PetType.OilPet].gameObject.SetActive(false);

            onClear?.Invoke();
        }

    }
}
