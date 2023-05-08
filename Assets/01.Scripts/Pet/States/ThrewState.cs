using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrewState : PetState {
    public override PetStateName StateName => PetStateName.Threw;

    public override void OnEnter() {
        pet.Rigid.constraints = RigidbodyConstraints.None;
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.isKinematic = false;
        pet.IsInputLock = false;

        StartCoroutine(EnableColl(pet.transform.position));

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
    }

    private void OnLanding() {
        pet.State.ChangeState((int)PetStateName.Landing);
    }

    private void OnRecall() {
        pet.State.ChangeState((int)PetStateName.Recall);
    }

    /// <summary>
    /// 펫이 Hold 상태에서 해제된 후 Distancec이상 멀어졌거나 time 만큼의 시간이 지났다면 콜라이더를 다시 활성화합니다.
    /// </summary>
    private IEnumerator EnableColl(Vector3 thrower, float distance = 2f, float time = 1f) {
        float timer = 0f;
        while ((thrower - transform.position).sqrMagnitude > Mathf.Pow(distance, 2) && timer <= time) {
            timer += Time.deltaTime;
            yield return null;
        }
        pet.Coll.enabled = true;
    }
}

