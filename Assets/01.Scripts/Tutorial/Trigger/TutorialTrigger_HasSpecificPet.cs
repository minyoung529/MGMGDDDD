using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_HasSpecificPet : TutorialTrigger
{
    [SerializeField]
    private PetType petType;

    protected override bool Condition(Transform player)
    {
        switch (petType)
        {
            case PetType.OilPet:
                return PetManager.Instance.GetMyPetByKind<OilPet>();
            case PetType.FirePet:
                return PetManager.Instance.GetMyPetByKind<FirePet>();
            case PetType.StickyPet:
                return PetManager.Instance.GetMyPetByKind<StickyPet>();
        }
        
        return false;
    }
}
