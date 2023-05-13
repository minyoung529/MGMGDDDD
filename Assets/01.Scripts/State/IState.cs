using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public int fence { get; set; }
    
    public abstract void SetUp(Transform root);
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public void Block()
    {
        ++fence;
    }

    public void UnBlock()
    {
        --fence;
    }
}