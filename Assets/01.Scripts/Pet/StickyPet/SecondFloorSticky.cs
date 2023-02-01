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
            StopGear();
        }
    }

    private void StopGear()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            EventManager.TriggerEvent(EventName.StopGear, dic);
            enabled = false;
        }
    }
}
