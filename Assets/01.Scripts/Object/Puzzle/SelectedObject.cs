using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectedObject : MonoBehaviour
{
    private static TutorialTrigger mouseGuide;
    private static OutlineScript interactObj;

    public static OutlineScript CurInteractObject;

    void Awake()
    {
        mouseGuide = gameObject.AddComponent<TutorialTrigger>();

        mouseGuide.SetAutoEnd(false);
        mouseGuide.SetTutorialType(TutorialType.InteractObject);
    }

    private void Update()
    {
        if (PetManager.Instance.GetSelectedPet() == null) return;
        CheckObject();
    }

    public void CheckObject()
    {
        if (CurInteractObject != null) return;

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

            if (selected.IsInteract) return;
            if (interactObj == selected) return;

            interactObj = selected;

            if (interactObj.GuideName.Length == 0)
            {
                mouseGuide.SetTutorialName("작동");
            }
            else
            {
                mouseGuide.SetTutorialName(interactObj.GuideName);
            }
            
            mouseGuide.StartTutorial();
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
            mouseGuide.EndTutorial();
        }
    }

    public static void SetInteractionObject()
    {
        CurInteractObject = interactObj;
        mouseGuide.EndTutorial();
    }

    public static OutlineScript GetInteract()
    {
        return interactObj;
    }
}