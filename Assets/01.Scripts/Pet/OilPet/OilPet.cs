using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;

public class OilPet : Pet
{
    [SerializeField] private OilSkillState skill;
    public OilSkillState SkillState { get { return skill; } }

    private void Start()
    {
        // skill = 
    }

}
