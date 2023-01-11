using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePet : Pet
{
    protected override void Skill()
    {
        Debug.Log(gameObject.name + " : Skill");
    }

}
