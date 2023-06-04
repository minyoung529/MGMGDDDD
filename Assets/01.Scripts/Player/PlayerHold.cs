using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(PlayerMove))]
public class PlayerHold : PlayerMono {
    [SerializeField] private float distance2Pet = 1f;
    [SerializeField] private Trajectory trajectory;
    [SerializeField] [Range(0, 90)] private float throwAngle;
    [SerializeField] private float throwPow;
    private Vector3 ThrowVector {
        get {
            return Quaternion.AngleAxis(throwAngle, -transform.right) * transform.forward * throwPow;
        }
    }

    private Sequence seq;
    private Pet holdingPet;
    private bool isHolding;

    private float defaultAngle;
    private float defaultPower;

    private void Awake() {
        InputManager.StartListeningInput(InputAction.PickUp_And_Drop, GetInput);
        InputManager.StartListeningInput(InputAction.Throw, GetInput);

        defaultAngle = throwAngle;
        defaultPower = throwPow;
    }

    public void SetThrowAngle(float value)
    {
        if (value > 90) value = 90;
        throwAngle= value;
    }
    
    public void SetThrowPower(float value)
    {
        throwPow = value;
    }
    
    public void SetDefault()
    {
        SetThrowPower(defaultPower);
        SetThrowAngle(defaultAngle);
    }

    private void GetInput(InputAction action, float value) {
        if (controller.Move.IsInputLock) return;
        controller.Move.IsInputLock = true;

        switch (action) {
            case InputAction.PickUp_And_Drop:
                if (!holdingPet)
                    PickUp();
                else
                    Drop();
                break;

            case InputAction.Throw:
                Throw();
                break;

            default:
                Debug.LogError($"올바르지 않은 입력이 감지되었습니다! 입력명:{action}");
                break;
        }
    }

    private void PickUp() {
        Dictionary<float, Pet> inDistance = new Dictionary<float, Pet>();
        foreach (Pet item in GameManager.Instance.Pets) { //각과 거리에 해당되는 펫만 거르기
            Vector3 dir = item.transform.position - transform.position;
            float dot = Vector3.Dot(transform.forward, dir.normalized);
            if (dir.magnitude <= distance2Pet && dot >= 0.5f) {
                inDistance.Add(dot, item);
            }
        }

        var pets = from item in inDistance
                   orderby item.Key descending
                   select item.Value;

        foreach (Pet item in pets) { //각도 순으로 집어지는지 확인
            item.Event.TriggerEvent((int)PetEventName.OnHold);
            if (item.State.CurStateIndex == (int)PetStateName.Held) {
                holdingPet = item;
                break;
            }
        }

        if (!holdingPet) {
            controller.Move.IsInputLock = false;
            return;
        }
        Physics.IgnoreCollision(controller.Coll, holdingPet.Coll, true);
        Vector3 petPos = holdingPet.transform.position;
        petPos.y = transform.position.y;
        transform.DOLookAt(petPos, 0.5f);
        controller.Rigid.velocity = Vector3.zero;
        controller.Move.ChangeState(PlayerStateName.PickUp);
    }

    private void Drop() {
        controller.Rigid.velocity = Vector3.zero;
        controller.Move.ChangeState(PlayerStateName.Drop);
    }

    private void Throw() {
        if (!holdingPet) {
            controller.Move.IsInputLock = false;
            return;
        }
        controller.Move.ChangeState(PlayerStateName.Throw);
    }

    private IEnumerator MovePetToHand() {
        isHolding = true;
        while (isHolding) {
            trajectory.DrawLine(holdingPet.transform.position, Quaternion.AngleAxis(throwAngle, -Vector3.right) * Vector3.forward * throwPow, holdingPet.Rigid.mass);
            holdingPet.transform.position =
                Vector3.Lerp(
                    controller.Anim.GetBoneTransform(HumanBodyBones.LeftHand).position,
                    controller.Anim.GetBoneTransform(HumanBodyBones.RightHand).position,
                    0.5f
                    )
                + transform.forward * 0.2f;
            yield return null;
        }
        trajectory.StopDraw();
    }

    #region Anim Events
    public void OnPickUp() {
        StartCoroutine(MovePetToHand());
    }

    public void OnDrop() {
        isHolding = false;
        
        seq = DOTween.Sequence();
        seq.Append(holdingPet.transform.DOMove(holdingPet.transform.position + transform.forward.normalized * 0.5f, 0.2f));
        seq.AppendCallback(() => {
            Physics.IgnoreCollision(controller.Coll, holdingPet.Coll, false);
            holdingPet.Event.TriggerEvent((int)PetEventName.OnDrop);
            holdingPet = null;
        });
    }

    public void OnThrow() {
        Vector3 dir = Vector3.zero;
        dir.y = controller.Rigid.velocity.y;
        controller.Rigid.velocity = dir;
        isHolding = false;
        holdingPet.Throw(ThrowVector);
        StartCoroutine(EnableCollision(holdingPet));
        holdingPet = null;
    }

    private IEnumerator EnableCollision(Pet holdingPet) {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(controller.Coll, holdingPet.Coll, false);
    }

    public void OnAnimEnd() {
        controller.Move.IsInputLock = false;
        controller.Move.ChangeState(PlayerStateName.DefaultMove);
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