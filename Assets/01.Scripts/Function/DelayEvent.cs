using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onCompelete;
    [SerializeField] private float timer = 0f;

    [SerializeField]
    private bool playOnAwake = false;

    void Start()
    {
        if (playOnAwake)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(timer);
        onCompelete?.Invoke();
    }
}
