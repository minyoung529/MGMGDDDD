using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeAbilityFloor : MonoBehaviour
{
    [SerializeField] PetType floorType;

    private void OnTriggerEnter(Collider other)
    {
        Pet pet = other.GetComponent<Pet>();
      //  if(pet.GetAbility != floorType) pet. 
    }
}
