using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffPet : MonoBehaviour
{

    public void On()
    {
            PetManager.Instance.BindingPet(PetType.OilPet)?.gameObject.SetActive(true);
            PetManager.Instance.BindingPet(PetType.FirePet)?.gameObject.SetActive(true);
            PetManager.Instance.BindingPet(PetType.StickyPet)?.gameObject.SetActive(true);
    }

    public void Off()
    {
        PetManager.Instance.BindingPet(PetType.OilPet)?.gameObject.SetActive(false);
        PetManager.Instance.BindingPet(PetType.FirePet)?.gameObject.SetActive(false);
        PetManager.Instance.BindingPet(PetType.StickyPet)?.gameObject.SetActive(false);
    }
}
