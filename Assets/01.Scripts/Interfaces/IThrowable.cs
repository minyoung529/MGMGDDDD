using UnityEngine;

public interface IThrowable
{
    public abstract Transform transform { get; }
    public abstract void OnHold();
    public abstract void OnDrop();
    public abstract void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse);
    public abstract void OnThrow();
    public abstract void OnLanding();
}