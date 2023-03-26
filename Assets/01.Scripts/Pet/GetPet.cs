using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (pet == null) return;
        if (PetManager.Instance.IsGet(pet)) return;

        pet.GetPet(gameObject.transform);
        OnGetPet?.Invoke();
        pet = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Define.PET_LAYER)  pet = other.GetComponent<Pet>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Define.PET_LAYER) pet = null;
    }
}
