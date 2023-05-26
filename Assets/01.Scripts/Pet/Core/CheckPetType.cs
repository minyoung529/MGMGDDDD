using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPetType : MonoBehaviour
{
    [SerializeField] PetType correctPetType;
    [SerializeField] Transform correctPos;
    [SerializeField] Transform failedPos;

    [SerializeField] UnityEvent correctEvent;
    [SerializeField] UnityEvent failEvent;

    private Pet inPet;

    public PetType GetInputPet => inPet.GetPetType;

    public void SetInPet(Pet p)
    {
        inPet = p;
    }
    public void SetCorrectPet(PetType type)
    {
        correctPetType = type;
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

    public void ActivePet(Transform arrivePos)
    {
        inPet.SetNavEnabled(false);
        inPet.transform.position = arrivePos.position;
        inPet.Rigid.velocity = Vector3.zero;
        inPet.Rigid.isKinematic = false;
        inPet.Coll.enabled = true;
        inPet.SetNavEnabled(true);
    }
}
