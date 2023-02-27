using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePet : Pet
{
    [SerializeField] GameObject fireBall;

    Fire fire;
    bool isOn = false;

    protected override void Awake()
    {
        base.Awake();
        fire = GetComponent<Fire>();
    }

    #region Set
    protected override void ResetPet()
    {
        base.ResetPet();

        isOn = false;
        fire.StopBurn();
    }
    #endregion

    #region Skill

    // Active skill
    protected override void Skill(InputAction inputAction, float value)
    {
        if (CheckSkillActive) return;
        base.Skill(inputAction, value);

        isOn = !isOn;
        if (isOn)
        {
            fire.Burn();
        }
        else
        {
            fire.StopBurn();
        }

    }


    private GameObject CreateFire()
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        FireBall fire = Instantiate(fireBall, spawnPoint, Quaternion.identity).GetComponent<FireBall>();
        return fire.gameObject;
    }

    #endregion
}
