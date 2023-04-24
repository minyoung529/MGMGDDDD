using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerCompo
{
    public PlayerController Controller { get; protected set; }

    public void SetController(PlayerController controller) {
        if (!Controller) {
            Debug.LogError("2개 이상의 PlayerController가 존재합니다");
            GameObject.Destroy(controller.gameObject);
            return;
        }
        Controller = controller;
    }
}