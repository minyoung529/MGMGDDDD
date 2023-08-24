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
        if (trigger.gameObject == other.gameObject)    // �¿��̸�
        {
            // TODO: ���� ��ġ��
            if (mainSpringObject.IsSticky)
            {
                mainSpringObject.OffSticky();
                Destroy(mainSpringObject.gameObject); // ���ֱ�

                EventManager.TriggerEvent(EventName.BossSuccess);
            }
        }
    }
}
