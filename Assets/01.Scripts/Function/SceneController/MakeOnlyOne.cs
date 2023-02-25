using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeOnlyOne : MonoBehaviour
{
    [SerializeField] MonoBehaviour scriptType;

    private void Awake()
    {
        if(FindObjectsOfType(scriptType.GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
