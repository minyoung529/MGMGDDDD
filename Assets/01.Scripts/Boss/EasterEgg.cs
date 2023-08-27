using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : HoldableObject
{
    [SerializeField]
    private ParticleSystem hitParticle;

    public bool IsThrowing { get; set; } = false;

    public override void OnDrop()
    {
        collider.enabled = true;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
    }

    public override void OnHold()
    {
        isHold = true;
        collider.enabled = false;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = true;
    }

    public override void OnDropFinish()
    {
        isHold = false;
        pet.Event.TriggerEvent((int)PetEventName.OnDrop);
    }

    public override void OnLanding()
    {
        isHold = false;
        IsThrowing = false;

        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        rigid.velocity = Vector3.zero;
    }

    public override void OnThrow()
    {
        IsThrowing = true;
        collider.enabled = true;
        rigid.constraints = RigidbodyConstraints.None;
    }

    public override void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        IsThrowing = true;
        isHold = false;
        collider.enabled = true;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.AddForce(force, forceMode);
    }

    public void Delete()
    {
        StartCoroutine(ParticlePlayAndDestroy());
    }

    private IEnumerator ParticlePlayAndDestroy()
    {
        hitParticle.Play();
        IsThrowing = false;

        yield return new WaitForSeconds(hitParticle.main.duration);

        Destroy(gameObject);
    }
}
