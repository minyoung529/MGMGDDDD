using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingEvent : MonoBehaviour
{
    [SerializeField] private float deadLine = 0;
    [SerializeField] private UnityEvent onFallDown;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= deadLine) {
            onFallDown.Invoke();
        }
    }
}
