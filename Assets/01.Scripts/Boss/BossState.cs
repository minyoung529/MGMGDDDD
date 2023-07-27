using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour, IState
{
    protected Boss boss;
    public abstract BossStateName StateName { get; }
    public int fence { get; set; }

    public virtual void SetUp(Transform root)
    {
        boss = root.GetComponent<Boss>();
        fence = 0;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}
