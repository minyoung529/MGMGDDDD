using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Pet))]
public class PetThrow : MonoBehaviour {
    [SerializeField] private float landingTime = 1f;
    [SerializeField] private LayerMask collisionLayer;

    private Pet pet = null;

    private void Awake() {
        pet = GetComponent<Pet>();
    }

    public void Throw(Vector3 force) {
        pet.Event.TriggerEvent((int)PetEventName.OnThrew);
        pet.Rigid.AddForce(force, ForceMode.Impulse);
    }

    private void WakeUp() {
        pet.Rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;

        transform.DOMove(transform.position + Vector3.up * 3f, 0.5f).OnComplete(() => {
            pet.Rigid.velocity = Vector3.zero;
        });
        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0), 0.5f);
    }

    private void OnLanding() {
        pet.Event.TriggerEvent((int)PetEventName.OnLanding);
    }
}