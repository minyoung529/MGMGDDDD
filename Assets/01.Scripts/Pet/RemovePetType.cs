using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePetType : MonoBehaviour
{
    [SerializeField] private PetType type;
    private Pet pet;

    private void GetPetComponent()
    {
        switch (type)
        {
            case PetType.None:
                break;
            case PetType.FirePet:
                pet = PetManager.Instance.GetPetByKind<FirePet>();
                break;
            case PetType.OilPet:
                pet = PetManager.Instance.GetPetByKind<OilPet>();
                break;
            case PetType.StickyPet:
                pet = PetManager.Instance.GetPetByKind<StickyPet>();
                break;
        }

    }

    public void RemovePet()
    {
        GetPetComponent();
        PetManager.Instance.DeletePet(pet);
    }
}
