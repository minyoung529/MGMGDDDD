using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialCameraActive : MonoBehaviour
{
    [SerializeField]
    private DialCameraDetector frontTrigger;

    [SerializeField]
    private DialCameraDetector backTrigger;

    private Transform player;

    [SerializeField]
    private UnityEvent onContact;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>().transform;

        frontTrigger.OnContact += onContact.Invoke;
        backTrigger.OnContact += onContact.Invoke;
    }

    void Update()
    {
        // front trigger�� �Ÿ��� �� �� �� => back trigger�� ���ش�

        bool isBackTriggerActive = IsFrontTriggerActive();

        frontTrigger.gameObject.SetActive(isBackTriggerActive);
        backTrigger.gameObject.SetActive(!isBackTriggerActive);
    }

    private bool IsFrontTriggerActive()
    {
        // Front�� Active�� ��
        // => Player�� Front�� �Ÿ��� �� ���� ��
        return (frontTrigger.transform.position - player.position).sqrMagnitude < (backTrigger.transform.position - player.position).sqrMagnitude;
    }
}
