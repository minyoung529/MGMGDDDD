using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerCompo
{
    public PlayerController Controller { get; protected set; }

    public void SetController(PlayerController controller) {
        if (!Controller) {
            Debug.LogError("2�� �̻��� PlayerController�� �����մϴ�");
            GameObject.Destroy(controller.gameObject);
            return;
        }
        Controller = controller;
    }
}