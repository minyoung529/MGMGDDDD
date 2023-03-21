using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveState : MonoBehaviour
{
    public abstract StateName StateName { get; }
    public PlayerMove Player { get; set; }

    /// <summary>
    /// PlayerMove에서 업데이트 시마다 호출
    /// </summary>
    public abstract void OnInput(Vector3 inputDir);
    public virtual void OnStateStart() {
    }
    public virtual void OnStateEnd(Action onChange) {
        //오버라이딩 후 여기에 하고 싶은 일을 적으면 됨 (형식을 꼭 지킬것)
        onChange?.Invoke();
    }
}
