using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeletePet : MonoBehaviour
{
    [SerializeField] PetType deletePetType = PetType.None;

    public void Trigger()
    {
        PetManager.Instance.DeletePet(deletePetType);
    }


}
