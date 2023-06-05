using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.Events;

public class RecallState : PetState
{
    public override PetStateName StateName => PetStateName.Recall;

    [SerializeField] private float sightRange = 20f;
    [SerializeField] private ParticleSystem flyParticlePref;
    [SerializeField] private ParticleSystem arriveParticlePref;
    [SerializeField] PlaySound recallSound;

    private ParticleSystem flyParticle = null;
    private ParticleSystem arriveParticle = null;

    [SerializeField]
    private UnityEvent onFlyEnd;

    public override void OnEnter()
    {
        CheckDistanceToPlayer();
        pet.Event.StartListening((int)PetEventName.OnThrew, OnThrew);
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnThrew, OnThrew);
    }

    public override void OnUpdate()
    {

    }

    private void Start()
    {
        flyParticle = Instantiate(flyParticlePref, pet.transform);
        arriveParticle = Instantiate(arriveParticlePref, pet.transform);
    }

    private void OnThrew()
    {
        pet.State.ChangeState((int)PetStateName.Threw);
    }

    private void CheckDistanceToPlayer() {
        if (Vector3.Distance(transform.position, pet.Player.position) <= sightRange &&
            pet.IsTargetOnRoute(pet.Player)) {
            pet.SetTargetPlayer();
            pet.State.ChangeState((int)PetStateName.Move);
            return;
        }
        Fly();
    }


    private void Fly()
    {
        pet.Event.TriggerEvent((int)PetEventName.OnFly);
        pet.SetNavEnabled(false);
        pet.Coll.enabled = false;
        pet.Rigid.isKinematic = true;

        // Default Color: White
        pet.Emission.EmissionOn();

        Vector3[] path = DrawBezier();

        flyParticle.Play();
        recallSound.Play();

        pet.transform.DOKill();
        pet.transform.DOLookAt(pet.Player.position, 0.5f);
        pet.transform.DOPath(path, 2f, PathType.CubicBezier).SetEase(Ease.InSine).OnComplete(() =>
        {
            pet.Emission.EmissionOff();
            flyParticle.Stop();
            onFlyEnd?.Invoke();
            arriveParticle.Play();
            pet.SetTargetPlayer();
            pet.Throw(Vector3.up * 300);

            pet.Event.TriggerEvent((int)PetEventName.OnFlyEnd);
        });
    }
    private Vector3[] DrawBezier()
    {
        Vector3 dest = pet.Player.position + (transform.position - pet.Player.position).normalized * 2f;
        dest = pet.GetNearestNavMeshPosition(dest) + Vector3.up * 1.5f;

        Vector3[] path = new Vector3[3];
        path[0] = dest + Vector3.up;
        path[1] = Vector3.Lerp(transform.position, path[0], 0.2f) + Vector3.up * 5f;
        path[2] = Vector3.Lerp(transform.position, path[0], 0.8f) + Vector3.up * 3f;

        return path;
    }
}
