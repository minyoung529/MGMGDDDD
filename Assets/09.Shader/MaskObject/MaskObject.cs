using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public GameObject[] maskObj;

    [SerializeField]
    private bool includeChild = false;

    private void Start()
    {
        for (int i = 0; i < maskObj.Length; i++)
        {
            if (includeChild)
            {
                MeshRenderer[] materials = maskObj[i].GetComponentsInChildren<MeshRenderer>();
                
                foreach (MeshRenderer renderer in materials)
                {
                    renderer.material.renderQueue = 3002;
                }
            }
            else
            {
                maskObj[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
            }
        }
    }
}
