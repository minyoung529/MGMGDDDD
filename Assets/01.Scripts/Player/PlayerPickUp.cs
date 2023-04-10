using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerPickUp : MonoBehaviour {
    [SerializeField] private float distance2Pet = 2;
    [SerializeField] private float throwPow;

    private Sequence seq;
    private PlayerMove playerMove;
    private Pet holdingPet;
    private bool isHolding;

    private void Awake() {
        playerMove = GetComponent<PlayerMove>();
        InputManager.StartListeningInput(InputAction.PickUp_And_Drop, GetInput);
        InputManager.StartListeningInput(InputAction.Throw, GetInput);
    }

    private void GetInput(InputAction action, float value) {
        if (playerMove.IsInputLock) return;
        playerMove.IsInputLock = true;
        playerMove.Rigid.velocity = Vector3.zero;

        switch (action) {
            case InputAction.PickUp_And_Drop:
                if (!holdingPet) {
                    holdingPet = FindPet();
                    if (!holdingPet) {
                        playerMove.IsInputLock = false;
                        break;
                    }
                    holdingPet.IsInputLock = true;
                    holdingPet.Coll.enabled = false;
                    Vector3 dest = CallPet(holdingPet);
                    StartCoroutine(WaitPet(dest, () => playerMove.ChangeState(StateName.PickUp)));
                }
                else
                    playerMove.ChangeState(StateName.Drop);
                break;
            case InputAction.Throw:
                if (!holdingPet) {
                    playerMove.IsInputLock = false;
                    return;
                }
                playerMove.ChangeState(StateName.Throw);
                break;
            default:
                Debug.LogError($"올바르지 않은 입력이 감지되었습니다! 입력명:{action}");
                break;
        }
    }

    #region PickUp 관련
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
        playerMove.IsInputLock = true;
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
                    playerMove.Anim.GetBoneTransform(HumanBodyBones.LeftHand).position,
                    playerMove.Anim.GetBoneTransform(HumanBodyBones.RightHand).position,
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
        holdingPet.Rigid.velocity = Vector3.zero;
        Vector3 dir = (transform.forward * 0.7f + Vector3.up).normalized;
        holdingPet.PetThrow.Throw(transform.position, dir * throwPow);
        holdingPet = null;
    }

    public void OnAnimEnd() {
        playerMove.IsInputLock = false;
        playerMove.ChangeState(StateName.DefaultMove);
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