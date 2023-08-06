using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_PetObtain : TutorialTrigger
{
    private Pet pet;

    private BoxCollider boxCollider;

    protected override void Start()
    {
        base.Start();
        pet ??= transform.parent.GetComponentInChildren<Pet>();

        if (pet == null || PetManager.Instance.Contain(pet))
        {
            Destroy(gameObject);
            return;
        }

        boxCollider = GetComponent<BoxCollider>();
        keyDownAction += InactiveTrigger;

        // 튜토리얼 플리킹 에러
        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf && Vector3.Distance(transform.position, pet.transform.position) > 0.1f)
        {
            transform.position = pet.transform.position;
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
