using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DetectObject : MonoBehaviour
{
    [SerializeField] private bool isOnce = false;
    [SerializeField] private UnityEvent onTrigger;

    private int trying = 0;
    private bool canTrigger = true;
    private const float coolTime = 0.7f;

    private void Awake()
    {
        trying = 0;
    }

    public void TriggerDetect()
    {
        if (isOnce && trying > 0) return;

        trying++;
        onTrigger?.Invoke();
        StartCoroutine(CoolTime());

        //EventParam param= new();
        //param["DetectPosition"] = transform;
        //EventManager.TriggerEvent(EventName.BossDetectObject, param);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(canTrigger) TriggerDetect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(canTrigger) TriggerDetect();
        }
    }

    private IEnumerator CoolTime()
    {
        canTrigger = false;
        yield return new WaitForSeconds(coolTime);
        canTrigger = true;
    }
}
