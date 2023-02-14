using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPet : MonoBehaviour
{
    public LayerMask petLayer;
    private float nearRadius = 2f;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, nearRadius, petLayer);
            if (colliders.Length <= 0) return;

            for (int i = 0; i < colliders.Length; i++)
            {
                Pet p = colliders[i].GetComponent<Pet>();
                if (p == null) continue;
                if (IsMine(p)) continue;
                p.GetPet(gameObject);
            }
        }
    }

    #region Boolean
    private bool IsMine(Pet p)
    {
        return p.IsGet;
    }
    #endregion

}
