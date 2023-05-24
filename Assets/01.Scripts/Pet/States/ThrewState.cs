using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrewState : PetState {
    [SerializeField] private LayerMask landingLayer;
    public override PetStateName StateName => PetStateName.Threw;

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
        CheckGround();
    }

    private void CheckGround() {
        if (pet.Rigid.velocity.y >= 0)
            return;
        RaycastHit hit;
        if (!Physics.BoxCast(
            transform.position + Vector3.up * 0.1f,
            new Vector3(0.5f, 0.05f, 0.5f),
            Vector3.down,
            out hit,
            Quaternion.identity,
            1f,
            landingLayer
            ))
            return;
        if (Vector3.Dot(Vector3.up, hit.normal) <= 0.4f)
            return;
        pet.Event.TriggerEvent((int)PetEventName.OnLanding);
    }

    private void OnLanding() {
        pet.State.ChangeState((int)PetStateName.Landing);
    }

    private void OnRecall() {
        pet.State.ChangeState((int)PetStateName.Recall);
    }
}

