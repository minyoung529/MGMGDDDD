using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetPet : MonoBehaviour
{
    public LayerMask petLayer;
    private float nearRadius = 10f;

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
        Debug.Log(colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            Pet p = colliders[i].GetComponent<Pet>();
            if (p == null || IsMine(p)) continue;
            p.GetPet(gameObject.transform);
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
