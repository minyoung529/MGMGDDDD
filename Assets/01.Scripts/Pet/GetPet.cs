using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GetPet : MonoBehaviour
{
    public LayerMask petLayer;

    [SerializeField]
    private float nearRadius = 30f;

    [SerializeField]
    private UnityEvent OnGetPet;

    private Pet pet = null;

    private void Awake()
    {
        StartListen();
    }
    private void OnDestroy()
    {
        StopListen();
    }
    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Get);
    }
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Interaction, Get);
    }


    private void Get(InputAction inputAction, float value)
    {
        Debug.Log("start");
        if (pet == null) return;
        Debug.Log("Not null");
        if (PetManager.Instance.IsGet(pet)) return;
        Debug.Log("get");

        pet.GetPet(gameObject.transform);
        OnGetPet?.Invoke();
        pet = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & petLayer) != 0)
        {
            pet = null;
            pet = other.GetComponent<Pet>();
            if(pet == null)
            {
                pet = other.transform.parent.GetComponent<Pet>();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & petLayer) != 0)
        {
            pet = null;
            pet = other.GetComponent<Pet>();
            if (pet == null)
            {
                pet = other.transform.parent.GetComponent<Pet>();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & petLayer) != 0)  pet = null;
    }
}
