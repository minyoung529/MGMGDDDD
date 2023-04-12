using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialCar : MonoBehaviour
{
    private DialPuzzleController controller;

    private void Awake()
    {
        // TEST
        controller = FindObjectOfType<DialPuzzleController>();
    }
    private void DeadPlayer()
    {
        EventParam eventParam = new();
        eventParam["position"] = controller.SpawnPosition;

        EventManager.TriggerEvent(EventName.PlayerDie, eventParam);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER_TAG))
        {
            DeadPlayer();
        }
    }
}
