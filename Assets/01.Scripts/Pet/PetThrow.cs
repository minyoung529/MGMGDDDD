using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Pet))]
public class PetThrow : MonoBehaviour {
    [SerializeField] private float landingTime = 1f;

    private Pet pet = null;
    private float elapsedLandingTime = 0;

    private bool isThrow = false;
    private bool isWake = false;

    private void Awake() {
        pet = GetComponent<Pet>();
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer != Define.BOTTOM_LAYER) return;

        if (isThrow) {
            if (collision.transform.CompareTag("Disrollable")) {
                WakeUp();
            }
            elapsedLandingTime += Time.deltaTime;
            if (elapsedLandingTime > landingTime) {
                elapsedLandingTime = 0f;
                WakeUp();
            }
        }
        else if (isWake) {
            OnLanding();
        }
    }

    public void Throw(Vector3 force) {
        isThrow = true;
        pet.Event.TriggerEvent((int)PetEventName.OnThrew);
        pet.Rigid.AddForce(force, ForceMode.Impulse);
    }

    private void WakeUp() {
        isThrow = false;
        isWake = true;

        pet.Rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;

        transform.DOMove(transform.position + Vector3.up * 3f, 0.5f).OnComplete(() => {
            pet.Rigid.velocity = Vector3.zero;
        });
        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0), 0.5f);
    }

    private void OnLanding() {
        isWake = false;
        pet.Event.TriggerEvent((int)PetEventName.OnLanding);
    }
}