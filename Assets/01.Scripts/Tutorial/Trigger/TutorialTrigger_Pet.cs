using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_Pet : TutorialTrigger
{
    private GetPet getPet = null;
    [SerializeField] private int petCount;

    protected override bool Condition(Transform player)
    {
        getPet ??= player.GetComponent<GetPet>();
        return getPet.PetCount >= petCount;
    }
}
