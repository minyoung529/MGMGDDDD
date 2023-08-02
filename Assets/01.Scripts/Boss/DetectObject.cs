using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObject : MonoBehaviour
{
    public void TriggerDetect()
    {
        EventParam param= new EventParam();
        param.AddEventParam("DetectPosition", transform.position);
        EventManager.TriggerEvent((int)BossEventName.DetectObject);
    }
}
