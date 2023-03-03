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
        Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, petLayer);


        Debug.Log("히히히히히");
        if (colliders.Length <= 0) return;
        Debug.Log(colliders[0]);
        for (int i = 0; i < colliders.Length; i++)
        {
            Pet p = colliders[i].GetComponent<Pet>();
            if (p == null) continue;
            if (IsMine(p)) continue;
            Debug.Log(colliders[i].name);
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

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, nearRadius);
    }
#endif
}
