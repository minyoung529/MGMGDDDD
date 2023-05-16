using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class RecallState : PetState {
    public override PetStateName StateName => PetStateName.Recall;

    [SerializeField] private float sightRange = 20f;
    [SerializeField] private ParticleSystem flyParticlePref;
    [SerializeField] private ParticleSystem arriveParticlePref;
    [SerializeField] PlaySound recallSound;

    private ParticleSystem flyParticle = null;
    private ParticleSystem arriveParticle = null;
    private NavMeshPath path;

    public override void OnEnter() {
        CheckDistanceToPlayer();
        pet.State.BlockState((int)PetStateName.Interact);
        pet.State.BlockState((int)PetStateName.Move);
        pet.Event.StartListening((int)PetEventName.OnThrew, OnThrew);
    }

    public override void OnExit() {
        pet.State.UnBlockState((int)PetStateName.Move);
        pet.State.UnBlockState((int)PetStateName.Interact);
        pet.Event.StopListening((int)PetEventName.OnThrew, OnThrew);
    }

    public override void OnUpdate() {

    }

    private void Start() {
        flyParticle = Instantiate(flyParticlePref, pet.transform);
        arriveParticle = Instantiate(arriveParticlePref, pet.transform);
        path = new NavMeshPath();
    }

    private void OnThrew() {
        pet.State.ChangeState((int)PetStateName.Threw);
    }

    private void CheckDistanceToPlayer() {
        if (pet.GetIsOnNavMesh() && Vector3.Distance(transform.position, pet.Player.position) <= sightRange &&
            NavMesh.CalculatePath(transform.position, pet.Player.position, NavMesh.AllAreas, path)) {
            if (Vector3.Distance(pet.Player.position, path.corners[path.corners.Length - 1]) <= 1f) {
                pet.SetTargetPlayer();
                pet.State.ChangeState((int)PetStateName.Move);
                return;
            }
        }
        Fly();
    }

    private void Fly() {
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
        pet.transform.DOPath(path, 2f, PathType.CubicBezier).SetEase(Ease.InSine).OnComplete(() => {
            pet.Emission.EmissionOff();
            flyParticle.Stop();
            arriveParticle.Play();
            pet.SetTargetPlayer();
            pet.PetThrow.Throw(Vector3.up * 300);
        });
    }

    private Vector3[] DrawBezier() {
        Vector3 dest = pet.Player.position + (transform.position - pet.Player.position).normalized * 2f;
        dest = pet.GetNearestNavMeshPosition(dest) + Vector3.up * 1.5f;

        Vector3[] path = new Vector3[3];
        path[0] = dest + Vector3.up;
        path[1] = Vector3.Lerp(transform.position, path[0], 0.2f) + Vector3.up * 5f;
        path[2] = Vector3.Lerp(transform.position, path[0], 0.8f) + Vector3.up * 3f;

        return path;
    }
}
