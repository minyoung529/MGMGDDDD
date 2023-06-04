using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPetType : MonoBehaviour
{
    [SerializeField] PetType correctPetType;

    [SerializeField] UnityEvent correctEvent;
    [SerializeField] UnityEvent failEvent;

    private CannonScript cannon;
    public PetType GetInputPet { get { return inPet.GetPetType; } }
    private Pet inPet => cannon.InPet;

    
    private void Awake()
    {
        cannon= GetComponent<CannonScript>();
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
                Debug.Log("Fail");
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
        inPet.State.ChangeState((int)PetStateName.Idle);
    }
}
