using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_PetBasic : TutorialTrigger
{
    [SerializeField] private int petCount;

    protected override bool Condition(Transform player)
    {
        return PetManager.Instance.PetCount >= petCount;
    }
}
