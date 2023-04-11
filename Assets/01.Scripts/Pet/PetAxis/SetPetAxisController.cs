using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPetAxisController : MonoBehaviour
{
    [SerializeField]
    private AxisControlType axisControlType;

    public void ChangeAxisControlType(StickyPet pet)
    {
        pet.AxisController.SetAxis(axisControlType);
    }

    public void BackOriginalAxis(StickyPet pet)
    {
        pet.AxisController.SetAxis(AxisControlType.None) ;
    }
}
