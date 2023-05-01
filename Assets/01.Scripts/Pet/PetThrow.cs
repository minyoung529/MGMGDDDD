using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Pet))]
public class PetThrow : MonoBehaviour {
    [SerializeField] private float landingTime = 1f;

    private Pet pet = null;
    private Action onComplete = null;
    private float elapsedLandingTime = 0;
    private bool isHolding = false;
    public bool IsHolding => isHolding;

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

    public void Hold(bool value, float distanceToEnableColl = 3f, float timeToEnableColl = 2f) {
        pet.Rigid.velocity = Vector3.zero;
        isHolding = value;
        pet.Rigid.isKinematic = value;
        pet.IsInputLock = value;
        if (value) { //들어올릴때 
            pet.Coll.enabled = false;
            pet.SetNavEnabled(false);
        }
        else { //내려놓을때
            StartCoroutine(EnableColl(transform.position, distanceToEnableColl, timeToEnableColl));
        }
    }

    public void Hold(bool value, Vector3 thrower, float distanceToEnableColl = 3f, float timeToEnableColl = 2f) {
        pet.Rigid.velocity = Vector3.zero;
        isHolding = value;
        pet.Rigid.isKinematic = value;
        pet.IsInputLock = value;
        if (value) { //들어올릴때 
            pet.Coll.enabled = false;
            pet.SetNavEnabled(false);
        }
        else { //내려놓을때
            StartCoroutine(EnableColl(thrower, distanceToEnableColl, timeToEnableColl));
        }
    }

    /// <summary>
    /// 펫이 Hold 상태에서 해제된 후 Distancec이상 멀어졌거나 time 만큼의 시간이 지났다면 콜라이더를 다시 활성화합니다.
    /// </summary>
    private IEnumerator EnableColl(Vector3 thrower, float distance = 3f, float time = 2f) {
        float timer = 0f;
        while ((thrower - transform.position).sqrMagnitude > Mathf.Pow(distance, 2) && timer <= time) {
            timer += Time.deltaTime;
            yield return null;
        }
        pet.Coll.enabled = true;
    }

    public void Throw(Vector3 thrower, Vector3 force, float distanceToEnable = 3f, Action onComplete = null) {
        isThrow = true;

        this.onComplete = onComplete;

        pet.Rigid.constraints = RigidbodyConstraints.None;
        Hold(false, thrower, distanceToEnable);
        pet.Rigid.AddForce(force, ForceMode.Impulse);
    }

    private void WakeUp() {
        isThrow = false;
        isWake = true;

        pet.Rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;

        pet.Coll.enabled = false;
        StartCoroutine(EnableColl(transform.position, 2f));
        transform.DOMove(transform.position + Vector3.up * 3f, 0.5f).OnComplete(() => {
            pet.Rigid.velocity = Vector3.zero;
            pet.enabled = true;
        });
        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0), 0.5f);
    }

    private void OnLanding() {
        isWake = false;

        pet.IsInputLock = false;
        pet.SetNavEnabled(true);
        onComplete?.Invoke();
        onComplete = null;
        pet.FindButton();
    }
}