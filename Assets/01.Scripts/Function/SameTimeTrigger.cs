using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SameTimeTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onClear;
    private Dictionary<PetType, Pet> triggerPets;

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
                triggerPets.Add(pet.GetPetType, pet);
                if (triggerPets.Count >= 3)
                {
                    onClear?.Invoke();

                    triggerPets[PetType.FirePet].gameObject.SetActive(false);
                    triggerPets[PetType.StickyPet].gameObject.SetActive(false);
                    triggerPets[PetType.OilPet].gameObject.SetActive(false);
                }
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Define.PET_LAYER)
        {
            Pet pet = other.GetComponent<Pet>();
            if (triggerPets.ContainsKey(pet.GetPetType))
            {
                triggerPets.Remove(pet.GetPetType);
            }
        }
    }
}
