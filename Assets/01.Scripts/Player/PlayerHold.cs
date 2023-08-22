using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;

[RequireComponent(typeof(PlayerMove))]
public class PlayerHold : PlayerMono
{
    [SerializeField] private float distance2Pet = 1f;
    [SerializeField] private Trajectory trajectory;
    [SerializeField][Range(0, 90)] private float throwAngle;
    [SerializeField] private float throwPow;
    [SerializeField] private LayerMask layer;

    [SerializeField] protected TutorialTrigger guide;
    private Vector3 ThrowVector
    {
        get
        {
            return Quaternion.AngleAxis(throwAngle, -transform.right) * transform.forward * throwPow;
        }
    }

    private Sequence seq;
    private HoldableObject holdableObject;
    private bool isHolding;

    private float defaultAngle;
    private float defaultPower;

    private void Awake()
    {
        InputManager.StartListeningInput(InputAction.PickUp_And_Drop, GetInput);

        // 컷신 시작시 내려놓기
        InputManager.StartListeningInput(InputAction.Throw, GetInput);
        EventManager.StartListening(EventName.PlayerDrop, DropEvent);

        defaultAngle = throwAngle;
        defaultPower = throwPow;
    }

    private void LateUpdate()
    {
        PlayRotation();

        HoldableObject holdable = GetHodlableObject(true);  // except pet

        if (!isHolding && holdable)
        {
            if (holdable.ExistGuideKey)
            {
                // 가이드 띄우기
                if (!guide.IsEnableTutorial)
                    guide?.StartTutorial();
            }
        }
        else if (!isHolding && holdable == null)
        {
            // 가이드 지우기
            if (guide.IsEnableTutorial)
                guide?.EndTutorial();
        }
    }

    public void SetThrowAngle(float value)
    {
        if (value > 90) value = 90;
        throwAngle = value;
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

    private void GetInput(InputAction action, float value)
    {
        if (controller.Move.IsInputLock) return;

        switch (action)
        {
            case InputAction.PickUp_And_Drop:
                {
                    HoldableObject obj = GetHodlableObject();

                    // Drop
                    if (holdableObject)
                    {
                        if (!holdableObject.GetIsPet())
                            holdableObject.OnDrop();

                        Drop();
                        controller.Move.LockInput();
                    }
                    else if (obj) // 잡을 수 있는 펫이 있으면 잡기
                    {
                        obj.OnHold();
                        PickUp(obj);
                        controller.Move.LockInput();
                    }
                    guide?.EndTutorial();

                    break;
                }

            case InputAction.Throw:
                {
                    if (holdableObject && holdableObject.CanThrew)
                    {
                        Throw();
                        controller.Move.LockInput();
                    }
                }
                break;

            default:
                Debug.LogError($"올바르지 않은 입력이 감지되었습니다! 입력명:{action}");
                break;
        }
    }

    private void PickUp(HoldableObject obj)
    {
        OnHold();

        holdableObject = obj;
        holdableObject.ListeningOnDestroy(OnHoldableObjectDestroyed);

        Physics.IgnoreCollision(controller.Coll, holdableObject.Coll, true);
        Vector3 petPos = holdableObject.transform.position;
        petPos.y = transform.position.y;
        transform.DOLookAt(petPos, 0.5f);
        controller.Rigid.velocity = Vector3.zero;
        controller.Move.ChangeState(PlayerStateName.PickUp);
    }

    private HoldableObject GetHodlableObject(bool exceptPet = false)
    {
        HoldableObject holdingPet = null;

        Dictionary<float, HoldableObject> inDistance = new Dictionary<float, HoldableObject>();

        Collider[] results = Physics.OverlapSphere(transform.position, distance2Pet, layer);
        if (results == null)
            return null;

        foreach (Collider coll in results)
        {
            HoldableObject obj = coll.GetComponent<HoldableObject>();

            if (obj)
            {
                Vector3 dir = obj.transform.position - transform.position;
                float dot = Vector3.Dot(transform.forward, dir.normalized);

                if (dir.magnitude <= distance2Pet && dot >= 0.5f)
                {
                    inDistance.Add(dot, obj);
                }
            }
        }

        var objs = from item in inDistance
                   orderby item.Key descending
                   select item.Value;

        foreach (HoldableObject item in objs)
        {
            // 펫 제외 옵션이면 넘긴다
            if (exceptPet && item.GetIsPet())
            {
                continue;
            }

            //각도 순으로 집어지는지 확인
            if (item.CanHold())
            {
                holdingPet = item;
                break;
            }
        }

        return holdingPet;
    }

    private void Drop()
    {
        OnExitHold();
        controller.Rigid.velocity = Vector3.zero;
        controller.Move.ChangeState(PlayerStateName.Drop);
    }
    private void DropEvent(EventParam eventParam = null)
    {
        if (holdableObject == null) return;
        OnExitHold();
        controller.Rigid.velocity = Vector3.zero;
        controller.Move.ChangeState(PlayerStateName.Drop);
    }

    private void Throw()
    {
        if (holdableObject == null)
        {
            controller.Move.UnLockInput();
            Debug.Log("Holdable Object is null");
            return;
        }

        Debug.Log("Throw");
        OnExitHold();
        controller.Move.ChangeState(PlayerStateName.Throw);
    }

    private IEnumerator MovePetToHand()
    {
        isHolding = true;
        while (isHolding)
        {
            if (holdableObject == null) yield return null;
            if (holdableObject.CanThrew)
            {
                trajectory.DrawLine(holdableObject.transform.position, Quaternion.AngleAxis(throwAngle, -Vector3.right) * Vector3.forward * throwPow, holdableObject.Rigid.mass);
            }
            holdableObject.transform.position =
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
    public void OnPickUp()
    {
        StartCoroutine(MovePetToHand());
    }

    // 애니메이션으로 재생
    public void OnDrop()
    {
        if (holdableObject == null) return;

        isHolding = false;

        seq = DOTween.Sequence();
        seq.Append(holdableObject.transform.DOMove(holdableObject.transform.position + transform.forward.normalized * 0.5f, 0.2f));
        seq.AppendCallback(() =>
        {
            holdableObject.OnDropFinish();
            Physics.IgnoreCollision(controller.Coll, holdableObject.Coll, false);
            holdableObject?.StopListeningOnDestroy(OnHoldableObjectDestroyed);
            holdableObject = null;
        });
    }

    public void OnThrow()
    {
        if (holdableObject == null) return;

        Vector3 dir = Vector3.zero;
        dir.y = controller.Rigid.velocity.y;
        controller.Rigid.velocity = dir;
        isHolding = false;
        holdableObject.Throw(ThrowVector);
        StartCoroutine(EnableCollision(holdableObject));
        holdableObject?.StopListeningOnDestroy(OnHoldableObjectDestroyed);
        holdableObject = null;
    }

    private IEnumerator EnableCollision(HoldableObject holdingPet)
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(controller.Coll, holdingPet.Coll, false);
    }

    public void OnAnimEnd()
    {
        controller.Move.UnLockInput();
        controller.Move.ChangeState(PlayerStateName.DefaultMove);
    }
    #endregion

    private void OnDisable()
    {
        seq.Kill();
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.PickUp_And_Drop, GetInput);
        InputManager.StopListeningInput(InputAction.Throw, GetInput);
        EventManager.StopListening(EventName.PlayerDrop, DropEvent);
    }

    private void PlayRotation()
    {
        if (holdableObject)
        {
            Vector3 forward = GameManager.Instance.MainCam.transform.forward;
            forward.y = 0f;

            holdableObject.transform.forward = forward;
            transform.forward = forward;
        }
    }

    private void OnHold()
    {
        PlayerController playerController = GameManager.Instance.PlayerController;

        for (int i = 0; i < 3; i++)
        {
            playerController.Camera.SetScreenX(i, 0.65f);
        }

        playerController.Move.IsBlockRotate = true;
        playerController.Camera.InactiveCrossHair();
    }

    private void OnExitHold()
    {
        PlayerController playerController = GameManager.Instance.PlayerController;

        for (int i = 0; i < 3; i++)
        {
            playerController.Camera.SetScreenX(i, 0.5f, 1f);
        }

        playerController.Move.IsBlockRotate = false;
        playerController.Camera.ActiveCrossHair();
    }

    private void OnHoldableObjectDestroyed()
    {
        holdableObject = null;
    }
}