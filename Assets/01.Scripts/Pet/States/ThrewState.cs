using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrewState : PetState {
    [SerializeField] private LayerMask landingLayer;
    private float bodyRadius = 0.7f;
    [SerializeField] private float timeToWake = 1f;
    public override PetStateName StateName => PetStateName.Threw;
    private float landingTime = 0f;

    public override void OnEnter() {
        pet.OnThrow();
        pet.Event.StartListening((int)PetEventName.OnLanding, OnLanding);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnExit() {
        pet.Event.StopListening((int)PetEventName.OnLanding, OnLanding);
        pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
    }

    public override void OnUpdate() {
        if(pet.transform.position.y <= -100f) {
            pet.Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);
        }
        UpdateLandingTime();
    }

    private void UpdateLandingTime() {
        if (!CheckGround()) {
            landingTime = 0;
            return;
        }
        landingTime += Time.deltaTime;
        if (landingTime >= timeToWake)
            pet.Event.TriggerEvent((int)PetEventName.OnLanding);
    }

    private bool CheckGround() {
        RaycastHit hit;
        if (!Physics.SphereCast(
            pet.transform.position + Vector3.up * bodyRadius, 
            bodyRadius, 
            Vector3.down, 
            out hit, 
            bodyRadius, 
            landingLayer)) return false;
        if (Vector3.Dot(Vector3.up, hit.normal) < 0.4f) return false;
        return true;
    }

    private void OnLanding() {
        pet.State.ChangeState((int)PetStateName.Landing);
    }

    private void OnRecall() {
        pet.State.ChangeState((int)PetStateName.Recall);
    }

    private void OnDrawGizmos() {

    }
}