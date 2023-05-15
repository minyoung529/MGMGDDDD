using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPetType : MonoBehaviour
{
    [SerializeField] PetType correctPetType;
    [SerializeField] UnityEvent correctEvent;
    [SerializeField] UnityEvent failEvent;

    private Pet inPet;

    public void SetInPet(Pet p)
    {
        inPet = p;
    }

    public void Check()
    {
        if(inPet != null)
        {
            if(inPet.GetPetType == correctPetType)
            {
                correctEvent?.Invoke();
            }
            else
            {
                failEvent?.Invoke();
            }
        }
    }
}
