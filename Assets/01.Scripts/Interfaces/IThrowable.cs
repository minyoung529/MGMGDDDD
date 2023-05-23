using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    public abstract Transform transform { get; }
    public abstract void OnHold();
    public abstract void OnDrop();
    public abstract void OnThrow();
    public abstract void OnLanding();
}