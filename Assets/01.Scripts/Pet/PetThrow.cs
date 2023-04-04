using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Pet))]
public class PetThrow : MonoBehaviour {
    [SerializeField] private float landingTime = 1f;

    private Pet pet;
    private bool isThrow;
    private float elapsedLandingTime = 0;

    private void Awake() {
        pet = GetComponent<Pet>();
    }

    private void OnCollisionStay(Collision collision) {
        if (!isThrow || collision.gameObject.layer != Define.BOTTOM_LAYER) return;

        elapsedLandingTime += Time.deltaTime;
        if (elapsedLandingTime > landingTime) {
            OnLanding();
            isThrow = false;
            elapsedLandingTime = 0f;
        }
    }

    public void Throw(Vector3 thrower, Vector3 force, float distanceToEnable = 3f) {
        isThrow = true;
        pet.Rigid.isKinematic = false;
        pet.Rigid.velocity = Vector3.zero;
        pet.Rigid.constraints = RigidbodyConstraints.None;
        pet.Rigid.AddForce(force, ForceMode.Impulse);
        StartCoroutine(EnableColl(thrower, distanceToEnable));
    }

    private IEnumerator EnableColl(Vector3 thrower, float distance) {
        while ((thrower - transform.position).sqrMagnitude > Mathf.Pow(distance, 2)) {
            yield return null;
        }
        pet.Coll.enabled = true;
    }

    public virtual void OnLanding() {
        pet.IsInputLock = false;
        pet.Rigid.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
        transform.DOMove(transform.position + Vector3.up * 3f, 1f);
        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0), 1f).OnComplete(() => {
            pet.SetNavEnabled(true);
            pet.FindButton();
        });
    }
}