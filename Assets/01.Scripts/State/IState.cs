using UnityEngine;

public interface IState
{
    public abstract void SetUp(Transform root);
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}