using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldablePet : HoldableObject
{
    private void Start()
    {
        collider = pet.Coll;
        rigid = pet.Rigid;
    }

    public override void OnDrop()
    {
        collider.enabled = true;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        pet.SetNavEnabled(true);
    }

    public override void OnHold()
    {
        isHold = true;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;
        collider.enabled = false;
        pet.SetNavEnabled(false);
    }

    public override void OnDropFinish()
    {
        isHold = false;
        pet.State.ChangeState((int)PetStateName.Held);  
        pet.Event.TriggerEvent((int)PetEventName.OnDrop);
    }

    public override void OnLanding()
    {
        isHold = false;
        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        rigid.velocity = Vector3.zero;
    }

    public override void OnThrow()
    {
        collider.enabled = true;
        rigid.constraints = RigidbodyConstraints.None;
    }

    public override void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.AddForce(force, forceMode);
        pet.Event.TriggerEvent((int)PetEventName.OnThrew);
    }

    public override bool CanHold()
    {
        pet.Event.TriggerEvent((int)PetEventName.OnHold);
        return (pet.State.CurStateIndex == (int)PetStateName.Held) && PetManager.Instance.Contain(pet);
    }
}
