using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalScript : MonoBehaviour
{
    public UnityEvent OnEnter;

    private void OnTriggerEnter(Collider other) {
        OnEnter.Invoke();
    }
}
