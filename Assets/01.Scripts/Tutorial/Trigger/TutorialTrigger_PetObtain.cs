using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_PetObtain : TutorialTrigger
{
    private Pet pet;

    private BoxCollider boxCollider;

    private void Start()
    {
        pet ??= transform.parent.GetComponentInChildren<Pet>();

        if (pet == null || PetManager.Instance.Contain(pet))
        {
            Destroy(gameObject);
            return;
        }

        boxCollider = GetComponent<BoxCollider>();

        if (PetManager.Instance.Contain(pet))
        {
            gameObject.SetActive(false);
        }
        else
        {
            keyDownAction += InactiveTrigger;
        }
    }

    protected override bool Condition(Transform player)
    {
        // 가지고 있지 않으면 TRUE
        return PetManager.Instance.Contain(pet) == false;
    }

    private void InactiveTrigger(InputAction action, float value)
    {
        if (pet == null || PetManager.Instance.IsGet(pet)) return;

        pet.GetPet(GameManager.Instance.PlayerController.transform);
        Inactive();
    }
}
