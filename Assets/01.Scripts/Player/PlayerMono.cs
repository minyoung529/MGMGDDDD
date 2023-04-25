using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMono : MonoBehaviour
{
    static protected PlayerController controller = null;
    public PlayerController Controller => controller;

    static public void SetController(PlayerController controller) {
        Debug.Log(controller.name);
        if (PlayerMono.controller) {
            Debug.LogError("2�� �̻��� PlayerController�� �����մϴ�");
            GameObject.Destroy(controller.gameObject);
            return;
        }
        PlayerMono.controller = controller;
    }
}