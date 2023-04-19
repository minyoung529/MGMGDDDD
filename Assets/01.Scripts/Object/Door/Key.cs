using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject useObject;
    [SerializeField] UnityEvent useEvent;
    [SerializeField] LayerMask targetLayer;

    private bool own = false;
    private bool around = false;

    public void GetKey(bool val)
    {
        own = val;
    }

    private void UseKey(InputAction act, float val)
    {
        if (!own || !around) return;
        useEvent?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == useObject) around = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == useObject)  around = false;
    }

    #region Input

    private void Start()
    {
        InputManager.StartListeningInput(InputAction.Interaction, UseKey);
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Interaction, UseKey);
    }

    #endregion

}
