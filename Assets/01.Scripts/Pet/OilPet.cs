using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPet : Pet
{
    protected override void Skill()
    {
        base.Skill();
        Debug.Log(gameObject.name + " : Skill");
    }
}
