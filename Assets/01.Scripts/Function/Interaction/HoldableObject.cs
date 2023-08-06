using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldableObject : MonoBehaviour
{
    [SerializeField]
    private bool canThrew = true;

    [SerializeField] protected Collider collider;
    public Collider Coll => collider;

    [SerializeField] protected Rigidbody rigid;
    public Rigidbody Rigid => rigid;

    protected bool isHold = false;
    public bool IsHold => isHold;

    public bool CanThrew => canThrew;

    protected Pet pet;

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
}
