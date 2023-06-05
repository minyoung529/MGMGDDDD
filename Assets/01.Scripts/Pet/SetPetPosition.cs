using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPetPosition : MonoBehaviour
{
    [SerializeField] Transform arrivePos;
    [SerializeField] PetType type;
    public void ActivePosition()
    {
        Pet pet=null;
        switch (type)
        {
            case PetType.FirePet:
                pet = PetManager.Instance.GetPetByKind<FirePet>();
                break;
            case PetType.StickyPet:
                pet = PetManager.Instance.GetPetByKind<StickyPet>();
                break;
            case PetType.OilPet:
                pet = PetManager.Instance.GetPetByKind<OilPet>();
                break;
        }
        if (pet == null) return;

        pet.SetNavEnabled(false);
        pet.transform.position = arrivePos.position;
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = false;
        pet.Coll.enabled = true;
        pet.SetNavEnabled(true);
        pet.State.ChangeState((int)PetStateName.Idle);
    }
}
