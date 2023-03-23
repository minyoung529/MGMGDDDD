using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerMove))]
public class PlayerPickUp : MonoBehaviour {
    [SerializeField] private float distance2Pet = 2;
    [SerializeField] private float throwPow;

    private PlayerMove playerMove;
    private Pet holdingPet;
    private bool isHolding;
    private bool isPlaying = false;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
        InputManager.StartListeningInput(InputAction.PickUp, GetInput);
    }

    private void GetInput(InputAction action, float value) {
        if (isPlaying) return;
        isPlaying = true;
        playerMove.Rigid.velocity = Vector3.zero;
        if (!holdingPet)
            PickUp();
        else
            Throw();
    }

    private void PickUp() {
        holdingPet = GameManager.Instance.GetNearest(transform, GameManager.Instance.Pets);
        if (!holdingPet) {
            isPlaying = false;
            return;
        }
        
        Vector3 dir = holdingPet.transform.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;
        dir = transform.position + dir * distance2Pet;
        dir.y = holdingPet.transform.position.y;

        holdingPet.SetDestination(dir);

        dir.y = transform.position.y;
        transform.DOLookAt(dir, 0.2f);

        StartCoroutine(WaitPet());
    }

    private IEnumerator WaitPet() {
        playerMove.IsInputLock = true;
        while (Vector3.Distance(holdingPet.transform.position, holdingPet.GetDestination()) >= 0.1f) {
            yield return null;
        }
        playerMove.IsInputLock = false;
        playerMove.ChangeState(StateName.PickUp);
    }

    public void PickUpStart() {
        HoldPet();
        StartCoroutine(SetPetPos());
    }

    public void PickUpEnd() {
        playerMove.ChangeState(StateName.DefaultMove);
        isPlaying = false;
    }

    private IEnumerator SetPetPos() {
        while (isHolding) {
            holdingPet.transform.position =
                Vector3.Lerp(
                    playerMove.Anim.GetBoneTransform(HumanBodyBones.LeftHand).position,
                    playerMove.Anim.GetBoneTransform(HumanBodyBones.RightHand).position,
                    0.5f
                    )
                + transform.forward * 0.2f;
            yield return null;
        }
    }

    private void Throw() {
        playerMove.ChangeState(StateName.Throw);
    }

    public void ThrowStart() {
        ThrowPet();
        holdingPet.StopNav(true);
        Vector3 dir = (transform.forward * 0.7f + Vector3.up).normalized;
        holdingPet.Rigid.AddForce(dir * throwPow, ForceMode.Impulse);
        holdingPet.OnThrow();
    }

    public void ThrowEnd() {
        playerMove.ChangeState(StateName.DefaultMove);
        holdingPet = null;
        isPlaying = false;
    }

    private void HoldPet() {
        isHolding = true;
        holdingPet.Rigid.isKinematic = true;
        holdingPet.Coll.enabled = false;
    }

    private void ThrowPet() {
        isHolding = false;
        holdingPet.Rigid.isKinematic = false;
        holdingPet.Coll.enabled = true;
        holdingPet.Rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }
}