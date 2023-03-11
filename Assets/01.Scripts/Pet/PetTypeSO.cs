using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet", menuName = "ScriptableObjects/Pet")]
public class PetTypeSO : ScriptableObject
{
    public PetType petType = PetType.NONE;

    public float throwPower = 0.0f;
    public float followDistance = 10.0f;
    public float skillDelayTime = 2.0f;

    public Sprite petUISprite;
}
