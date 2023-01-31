using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorSticky : MonoBehaviour
{
    [SerializeField] bool isOneCollision = false;
    Dictionary<string, object> dic = new Dictionary<string, object>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Gear"))
        {
            Debug.Log("Gear");
            EventManager.TriggerEvent(EventName.StopGear, dic);
            enabled = false;
        }
    }
}
