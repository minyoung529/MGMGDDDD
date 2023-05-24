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

    Action clearTriggerAction;
    Action outPetAction;

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

    private void OutPet(Pet p)
    {
        SetHologram(p.GetPetType, true);
        
        p.Event.StopListening((int)PetEventName.OnArrive, clearTriggerAction);
        p.Event.StopListening((int)PetEventName.OnRecallKeyPress, outPetAction);
        outPetAction = null;

        triggerPets.Remove(p.GetPetType);

    }

    private void InnerPet(Pet p)
    {
        triggerPets.Add(p.GetPetType, p);

        p.SetTarget(holograms[((int)p.GetPetType)-1].transform);

        clearTriggerAction = ()=>ClearTrigger(p);
        outPetAction = () => OutPet(p);

        p.Event.StartListening((int)PetEventName.OnArrive, clearTriggerAction);
        p.Event.StartListening((int)PetEventName.OnRecallKeyPress, outPetAction);
    }

    private void ClearTrigger(Pet p)
    {
        SetHologram(p.GetPetType, false);

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

    private void SetHologram(PetType type, bool active = false)
    {
        holograms[((int)type) - 1].gameObject.SetActive(active);

    }
}
