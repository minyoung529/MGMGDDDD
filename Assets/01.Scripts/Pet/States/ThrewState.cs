using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrewState : PetState
{
    [SerializeField] private LayerMask landingLayer;
    private float bodyRadius = 0.7f;
    [SerializeField] private float timeToWake = 1f;
    public override PetStateName StateName => PetStateName.Threw;
    private float landingTime = 0f;

    public override void OnEnter()
    {
        pet.OnThrow();
        pet.Event.StartListening((int)PetEventName.OnLanding, OnLanding);
        //pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StartListening((int)PetEventName.OnHold, OnHold);
        pet.Event.StartListening((int)PetEventName.OnOffPing, PingUp);
    }

    public override void OnExit()
    {
        pet.Event.StopListening((int)PetEventName.OnLanding, OnLanding);
        //pet.Event.StartListening((int)PetEventName.OnRecallKeyPress, OnRecall);
        pet.Event.StopListening((int)PetEventName.OnHold, OnHold);
        pet.Event.StopListening((int)PetEventName.OnOffPing, PingUp);
    }

    private void PingUp()
    {
        pet.StopPing();
    }

    public override void OnUpdate()
    {
        if (pet.transform.position.y <= -100f)
        {
            pet.Event.TriggerEvent((int)PetEventName.OnRecallKeyPress);
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, bodyRadius);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("Disrollable"))
            {
                pet.Event.TriggerEvent((int)PetEventName.OnLanding);
                return;
            }
        }

        UpdateLandingTime();
    }

    private void UpdateLandingTime()
    {
        if (!CheckGround())
        {
            landingTime = 0;
            return;
        }
        landingTime += Time.deltaTime;
        if (landingTime >= timeToWake)
        {
            pet.Event.TriggerEvent((int)PetEventName.OnLanding);
        }
    }

    private bool CheckGround()
    {
        if (!Physics.Raycast(
            pet.transform.position,
            Vector3.down,
            bodyRadius,
            landingLayer)) return false;
        // if (Vector3.Dot(Vector3.up, hit.normal) < 0.4f) return false;
        return true;
    }

    private void OnLanding()
    {
        pet.State.ChangeState((int)PetStateName.Landing);
    }

    private void OnHold()
    {
        pet.State.ChangeState((int)PetStateName.Held);
    }

    private void OnDrawGizmos()
    {
        if (pet)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pet.transform.position, bodyRadius);
        }
    }
}