using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_PetObtain : TutorialTrigger
{
    private Pet pet;
    bool isObtain = false;

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

        // Ʃ�丮�� �ø�ŷ ����
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
        // ������ ���� ������ TRUE
        return PetManager.Instance.Contain(pet) == false;
    }

    private void InactiveTrigger(InputAction action, float value)
    {
        if (pet == null || PetManager.Instance.IsGet(pet)) return;
        if (isObtain) return;

        pet.GetPet(GameManager.Instance.PlayerController.transform);
        Inactive();

        InputManager.StopListeningInput(InputAction.Interaction, keyDownAction);
        Destroy(gameObject);

        isObtain = true;
    }
}
