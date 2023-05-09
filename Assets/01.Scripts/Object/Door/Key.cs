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

    [SerializeField] private int activeIdx = 0;
    [SerializeField] private Color color;

    private bool own = false;
    private bool around = false;
    private StickyPet stickyPet;

    #region Property
    public int ActiveIdx => activeIdx;
    public Color Color => color;
    #endregion

    public void GetKey()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, (1<<Define.PET_LAYER));
        foreach(Collider col in colliders)
        {
            StickyPet sticky = col.GetComponent<StickyPet>();
            if(sticky)
            {
                stickyPet = sticky;
                own = true;
                break;
            }
         }
    }

    private void UseKey(InputAction act, float val)
    {
        if (!own || !around) return;

        own = false;
        //stickyPet?.NotSticky();
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
