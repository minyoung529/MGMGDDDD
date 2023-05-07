using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_PetObtain : TutorialTrigger
{
    private Pet pet;

    private void Awake()
    {
        pet ??= transform.parent.GetComponentInChildren<Pet>();
    }

    protected override bool Condition(Transform player)
    {
        // ������ ���� ������ TRUE
        return PetManager.Instance.Contain(pet) == false;
    }

    protected override void OnStartTrigger()
    {
        Destroy(gameObject);
    }
}
