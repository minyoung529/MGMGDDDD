using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : MoveState
{
    public override PlayerStateName StateName => PlayerStateName.Fall;

    public float MaxSpeed { get; private set; }

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float brake = 5f;
    [SerializeField] private float accel = 1f;

    public override void OnInput(Vector3 inputDir)
    {
        if (inputDir.sqrMagnitude <= 0)
        {
            Stop();
            return;
        }

        Player.Accelerate(inputDir, accel, brake, MaxSpeed);
        Player.SetRotate(inputDir, rotateTime);
    }

    private void Stop()
    {
        MaxSpeed = walkSpeed;
        Player.Decelerate(brake);
    }
}
