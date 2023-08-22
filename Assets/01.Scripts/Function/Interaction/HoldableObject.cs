using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldableObject : MonoBehaviour
{
    [SerializeField]
    private bool canThrew = true;

    [SerializeField] protected Collider collider;

    [SerializeField] protected Rigidbody rigid;

    protected bool isHold = false;

    [SerializeField]
    protected bool existGuideKey = false;

    protected Pet pet;



    #region Property
    public Collider Coll => collider;
    public Rigidbody Rigid => rigid;
    public bool IsHold => isHold;
    public bool CanThrew => canThrew;
    public bool ExistGuideKey => existGuideKey;
    #endregion

    private Action onDestroy;

    private void Awake()
    {
        pet = GetComponent<Pet>();
    }

    public abstract void OnHold();
    public abstract void OnDrop();
    public virtual void OnDropFinish() { }
    public virtual void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse) { }
    public virtual void OnThrow() { }
    public abstract void OnLanding();

    public virtual bool CanHold()
    {
        return !isHold;
    }

    public bool GetIsPet()
    {
        return pet;
    }

    public void ListeningOnDestroy(Action action)
    {
        onDestroy += action;
    }

    public void StopListeningOnDestroy(Action action)
    {
        onDestroy -= action;
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
