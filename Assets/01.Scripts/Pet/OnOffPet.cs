using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffPet : MonoBehaviour
{
    
    public void On()
    {
        for(int i=0;i<PetManager.Instance.PetCount;i++)
        {
            PetManager.Instance.GetPetList[i]?.gameObject.SetActive(true);
        }
    }

    public void Off()
    {
        for(int i=0;i<PetManager.Instance.PetCount;i++)
        {
            PetManager.Instance.GetPetList[i]?.gameObject.SetActive(false);
        }
    }

}
