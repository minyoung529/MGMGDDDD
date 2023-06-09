using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeOnlyOne : MonoBehaviour
{
    [SerializeField] MonoBehaviour scriptType;

    private void Awake()
    {
        Object[] objs = FindObjectsOfType(scriptType.GetType());

        if(objs == null || objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
