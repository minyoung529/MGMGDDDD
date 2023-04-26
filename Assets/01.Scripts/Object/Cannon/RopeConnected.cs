using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeConnected : MonoBehaviour
{
    private Fire connectObj;

    private void OnTriggerEnter(Collider other)
    {
        Fire fire = other.GetComponent<Fire>();
        if (fire != null) connectObj = fire;
    }

    [ContextMenu("Trigger")]
    public void TriggerConnect()
    {
        if (connectObj == null) return;

        connectObj.Burn();
    }
}
