using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerHold : PlayerMono
{
    [SerializeField] private float distance2Pet = 1f;
    [SerializeField] private float throwPow;

    private Sequence seq;
    private Pet holdingPet;
    private bool isHolding;

    private void Awake() {
        InputManager.StartListeningInput(InputAction.PickUp_And_Drop, GetInput);
        InputManager.StartListeningInput(InputAction.Throw, GetInput);
    }

    private void GetInput(InputAction action, float value) {
        if (controller.Move.IsInputLock) return;
        controller.Move.IsInputLock = true;
        controller.Rigid.velocity = Vector3.zero;

        switch (action) {
            case InputAction.PickUp_And_Drop:
                if (!holdingPet)
                    PickUp();
                else
                    controller.Move.ChangeState(StateName.Drop);
                break;
            case InputAction.Throw:
                if (!holdingPet) {
                    controller.Move.IsInputLock = false;
                    return;
                }
                controller.Move.ChangeState(StateName.Throw);
                break;
            default:
                Debug.LogError($"올바르지 않은 입력이 감지되었습니다! 입력명:{action}");
                break;
        }
    }

    #region PickUp 관련
    private void PickUp() {
        holdingPet = FindPet();
        if (!holdingPet || !holdingPet.GetIsOnNavMesh()) {
            controller.Move.IsInputLock = false;
            return;
        }
        holdingPet.IsInputLock = true;
        holdingPet.Coll.enabled = false;
        Vector3 dest = CallPet(holdingPet);
        if (Vector3.Distance(dest, transform.position) > distance2Pet + 1f) {
            holdingPet = null;
            controller.Move.IsInputLock = false;
            return;
        }
        StartCoroutine(WaitPet(dest, () => controller.Move.ChangeState(StateName.PickUp)));
    }

    public Pet FindPet() {
        Pet pet = PetManager.Instance.GetSelectedPet();
        if (!pet || !pet.GetIsOnNavMesh()) {
            return null;
        }
        return pet;
    }

    public Vector3 CallPet(Pet pet) {
        Vector3 dir = pet.transform.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;
        dir = transform.position + dir * distance2Pet;
        dir.y = pet.transform.position.y;

        pet.SetDestination(dir);

        return pet.GetDestination(); 
    }

    private IEnumerator WaitPet(Vector3 destination, Action onArrive) {
        controller.Move.IsInputLock = true;
        destination.y = transform.position.y;
        transform.DOLookAt(destination, 0.2f);
        while (Vector3.Distance(holdingPet.transform.position, holdingPet.GetDestination()) > 0.5f) {
            yield return null;
        }
        onArrive?.Invoke();
    }

    private IEnumerator MovePetToHand() {
        isHolding = true;
        holdingPet.SetNavEnabled(false);
        while (isHolding) {
            holdingPet.transform.position =
                Vector3.Lerp(
                    controller.Anim.GetBoneTransform(HumanBodyBones.LeftHand).position,
                    controller.Anim.GetBoneTransform(HumanBodyBones.RightHand).position,
                    0.5f
                    )
                + transform.forward * 0.2f;
            yield return null;
        }
    }
    #endregion

    #region Anim Events
    public void OnPickUp() {
        StartCoroutine(MovePetToHand());
    }

    public void OnDrop() {
        isHolding = false;
        holdingPet.Rigid.isKinematic = false;
        seq = DOTween.Sequence();
        seq.Append(holdingPet.transform.DOMove(holdingPet.transform.position + transform.forward.normalized * 0.5f, 0.2f));
        seq.AppendCallback(() => {
            holdingPet.Coll.enabled = true;
            holdingPet.SetNavEnabled(true);
            holdingPet = null;
        });
    }

    public void OnThrow() {
        isHolding = false;
        Vector3 dir = (transform.forward * 0.7f + Vector3.up).normalized;
        holdingPet.PetThrow.Throw(transform.position, dir * throwPow);
        holdingPet = null;
    }

    public void OnAnimEnd() {
        controller.Move.IsInputLock = false;
        controller.Move.ChangeState(StateName.DefaultMove);
    }
    #endregion

    private void OnDisable() {
        seq.Kill();
    }

    private void OnDestroy() {
        InputManager.StopListeningInput(InputAction.PickUp_And_Drop, GetInput);
        InputManager.StopListeningInput(InputAction.Throw, GetInput);
    }
}