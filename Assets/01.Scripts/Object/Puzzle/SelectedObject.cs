using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectedObject : MonoBehaviour
{
    public bool IsInteraction { get { return interactionObj != null; } }
    public GameObject InteractiveObj { get { return interactionObj.gameObject; } }
    private OutlineScript interactionObj = null;

    private void Update()
    {
        if (PetManager.Instance.GetSelectedPet() == null) return;

        CheckObject();
    }

    public void CheckObject()
    {
        RaycastHit hit;
        Ray ray = GameManager.Instance.MainCam.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            OutlineScript selected = hit.collider.GetComponent<OutlineScript>();
            Pet pet = PetManager.Instance.GetSelectedPet();

            if (selected == null || pet == null)
            {
                OffInteration();
                return;
            }
            if ((selected.PetType.GetHashCode() & pet.GetPetType.GetHashCode()) == 0)
            {
                OffInteration();
                return;
            }

            pet.IsInteraction = true;
            interactionObj = selected;
            interactionObj.SetColor(pet.petColor);
            interactionObj.OnOutline();
        }
        else
        {
            OffInteration();
        }
    }

    public void OffInteration()
    {
        if (interactionObj != null)
        {
            for (int i = 0; i < PetManager.Instance.PetCount; i++)
            {
                PetManager.Instance.GetPetList[i].IsInteraction = false;
            }
            interactionObj.OffOutline();
            interactionObj = null;
        }
    }


}