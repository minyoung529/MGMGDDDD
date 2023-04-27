using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeConnected : MonoBehaviour
{
    private Fire connectObj;
    private bool isConnect = false;

    private void OnTriggerStay(Collider other)
    {
        Fire fire = other.GetComponent<Fire>();
        if (fire != null)
        {
            isConnect= true;
            connectObj = fire;
        }
    }

    [ContextMenu("Trigger")]
    public void TriggerConnect()
    {
        if (connectObj == null || !isConnect) return;

            isConnect= false;
        connectObj.Burn();
    }
}
