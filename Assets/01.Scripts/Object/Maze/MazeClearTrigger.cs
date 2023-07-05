using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MazeClearTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] UnityEvent exitEvent;

    private Dictionary<PetType, Pet> clearPet = new Dictionary<PetType, Pet>();

    private void EnterGameOver(Pet clear)
    {
        if (clearPet.ContainsKey(clear.GetPetType)) return;
        clearPet.Add(clear.GetPetType, clear);
        clear.IsMovePointLock= true;
        if (clearPet.Count >= 2) ExitGame();
    }
    private void ExitGameOver(Pet exitPet)
    {
        clearPet.Remove(exitPet.GetPetType);
    }

    private void ExitGame()
    {
        exitEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            Pet pet = other.GetComponent<Pet>();
            if(pet != null) EnterGameOver(pet);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            Pet pet = other.GetComponent<Pet>();
            if(pet != null) ExitGameOver(pet);
        }
    }
}
