using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : HoldableObject
{
    [SerializeField]
    private ParticleSystem hitParticle;

    [SerializeField]
    private ParticleSystem eggParticle;

    public bool IsThrowing { get; set; } = false;
    public int SpawnIndex { get; set; }

    [SerializeField] private PlaySound hitSound;

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

    private void OnCollisionEnter(Collision collision)
    {
        //if(IsThrowing && collision.gameObject.CompareTag("Floor"))
        //{
        //    OnLanding();
        //}
    }

    public override void OnLanding()
    {
        //isHold = false;
        IsThrowing = false;

        rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        rigid.velocity = Vector3.zero;

        //eggParticle.Play();
        //eggParticle.transform.SetParent(null);
        //eggParticle.transform.localScale = Vector3.one;

        //Destroy(eggParticle, eggParticle.main.duration);
        //Destroy(gameObject);
    }

    public override void OnThrow()
    {
        //IsThrowing = true;
        IsThrowing = true;
        collider.enabled = true;
        rigid.constraints = RigidbodyConstraints.None;
    }

    public override void Throw(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        //IsThrowing = true;
        isHold = false;
        collider.enabled = true;
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = false;
        rigid.AddForce(force, forceMode);

        rigid.constraints = RigidbodyConstraints.None;

    }

    public void Delete()
    {
        StartCoroutine(ParticlePlayAndDestroy());
    }

    private IEnumerator ParticlePlayAndDestroy()
    {
        hitParticle.Play();
        hitSound.Play();
        //IsThrowing = false;

        yield return new WaitForSeconds(hitParticle.main.duration);

        Destroy(gameObject);
    }
}
