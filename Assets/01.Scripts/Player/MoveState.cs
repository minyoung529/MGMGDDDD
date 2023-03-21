using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState : MonoBehaviour
{
    public abstract StateName StateName { get; }
    public PlayerMove Player { get; set; }

    /// <summary>
    /// PlayerMove���� ������Ʈ �ø��� ȣ��
    /// </summary>
    public abstract void OnInput(Vector3 inputDir);
    public virtual void OnStateStart() {
    }
    public virtual void OnStateEnd(Action onChange) {
        //�������̵� �� ���⿡ �ϰ� ���� ���� ������ �� (������ �� ��ų��)
        onChange?.Invoke();
    }
}
