using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMono : MonoBehaviour
{
    static protected PlayerController controller = null;
    public PlayerController Controller => controller;

    static public void SetController(PlayerController controller) {
        Debug.Log(controller.name);
        if (PlayerMono.controller) {
            Debug.LogError("2개 이상의 PlayerController가 존재합니다");
            GameObject.Destroy(controller.gameObject);
            return;
        }
        PlayerMono.controller = controller;
    }
}