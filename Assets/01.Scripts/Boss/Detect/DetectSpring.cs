using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectSpring : MonoBehaviour
{
    [SerializeField]
    private Sticky mainSpringObject;

    [SerializeField]
    private GameObject trigger;

    private void OnTriggerEnter(Collider other)
    {
        if (trigger.gameObject == other.gameObject)    // 태엽이면
        {
            // TODO: 로직 고치기
            if (mainSpringObject.IsSticky)
            {
                mainSpringObject.OffSticky();
                Destroy(mainSpringObject.gameObject); // 없애기

                EventManager.TriggerEvent(EventName.BossSuccess);
            }
        }
    }
}
