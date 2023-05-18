using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class SelectedObject : MonoBehaviour
{

    private OutlineScript interactObj;

    public static OutlineScript CurInteractObject;

    private void Update()
    {
        if (PetManager.Instance.GetSelectedPet() == null) return;
        CheckObject();
    }

    public void CheckObject()
    {
        RaycastHit hit;
        Ray ray = GameManager.Instance.MainCam.ViewportPointToRay(Vector2.one * 0.5f);

    //    Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);

        if (Physics.Raycast(ray, out hit, 100f))
        {
          //  Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
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

            if (selected.IsInteract) return;

            CurInteractObject = interactObj;
            interactObj = selected;
            interactObj.SetColor(pet.petColor);
            interactObj.OnOutline();
        }
        else
        {
            OffInteration();
        }
    }

    public void OffInteration()
    {
        if (interactObj != null)
        {
            interactObj.OffOutline();
            interactObj = null;
        }
    }
}