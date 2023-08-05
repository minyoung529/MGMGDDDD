using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoveState : MoveState
{
    #region 속도, 방향
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float brake = 5f;
    [SerializeField] private float accel = 1f;

    [SerializeField] private float oilWalkWeight = 1.2f;
    [SerializeField] private float oilSprintWeight = 1.2f;

    public float MaxSpeed { get; private set; }
    #endregion

    #region 애니메이션
    private int hash_bWalk = Animator.StringToHash("bWalk");
    private int hash_bSprint = Animator.StringToHash("bSprint");
    private int hash_tStop = Animator.StringToHash("tStop");
    #endregion

    #region abstarct 구현 부분
    public override PlayerStateName StateName => PlayerStateName.SlowMove;

    public override void OnInput(Vector3 inputDir)
    {
        if (inputDir.sqrMagnitude <= 0)
        {
            Stop();
            return;
        }
        Player.Controller.Anim.SetBool(hash_bWalk, true);
        Player.Accelerate(inputDir, accel, brake, MaxSpeed);
        Player.SetRotate(inputDir, rotateTime);
    }
    #endregion

    private void Start()
    {
        // TEST
        GameManager.Instance.PlayerController.Move.ChangeState(PlayerStateName.SlowMove);
    }

    public override void OnStateStart()
    {
        base.OnStateStart();
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
        InputManager.StartListeningInput(InputAction.Sprint, Sprint);
    }

    public override void OnStateEnd(Action onChange)
    {
        base.OnStateEnd(onChange);
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }


    private void Sprint(InputAction action, float param)
    {
        if (!Player.Controller.Anim.GetBool(hash_bWalk)) return;
        bool isSprint = Player.Controller.Anim.GetBool(hash_bSprint);
        Player.Controller.Anim.SetBool(hash_bSprint, !isSprint);
        MaxSpeed = !isSprint ? sprintSpeed : walkSpeed;
    }

    private void Stop()
    {
        if (Player.Controller.Anim.GetBool(hash_bSprint) && Player.CurSpeed >= sprintSpeed)
        {
            Player.Controller.Anim.SetTrigger(hash_tStop);
            Player.LockInput(0.1f);
        }
        MaxSpeed = walkSpeed;
        Player.Controller.Anim.SetBool(hash_bSprint, false);
        Player.Controller.Anim.SetBool(hash_bWalk, false);
        Player.Decelerate(brake);
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Sprint, Sprint);
    }
}
