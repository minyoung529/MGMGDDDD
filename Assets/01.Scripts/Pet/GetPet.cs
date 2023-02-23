using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetPet : MonoBehaviour
{
    public LayerMask petLayer;
    private float nearRadius = 2f;

    private int petCount;
    public int PetCount => petCount;

    [SerializeField]
    private UnityEvent OnGetPet;

    private void Start()
    {
        StartListen();
    }

    private void OnDestroy()
    {
        InputManager.StopListeningInput(InputAction.Interaction, Get);
    }

    private void StartListen()
    {
        InputManager.StartListeningInput(InputAction.Interaction, Get);
    }


    private void Get(InputAction inputAction, float value)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, petLayer);
        if (colliders.Length <= 0) return;

        for (int i = 0; i < colliders.Length; i++)
        {
            Pet p = colliders[i].GetComponent<Pet>();
            if (p == null) continue;
            if (IsMine(p)) continue;
            p.GetPet(gameObject);
            petCount++;
            OnGetPet?.Invoke();
        }
    }

    #region Boolean
    private bool IsMine(Pet p)
    {
        return p.IsGet;
    }
    #endregion

}
