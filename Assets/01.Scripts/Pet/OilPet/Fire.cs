using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fire : MonoBehaviour
{
    float burningTime = 0;
    bool isBurn = false;
    Material mat;

    public bool IsBurn { get { return isBurn; } set { isBurn = value; } }

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        isBurn= false;
    }

    public void Burn()
    {
        isBurn = true;
        mat.color = Color.red;
        tag = "Fire";
    }

    public void DeleteBurn()
    {
        isBurn = true;
        mat.color = Color.red;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isBurn)
        {
            Fire f = other.GetComponent<Fire>();
            if (f != null)
            {
                if (f.isBurn) return;
                f.DeleteBurn();
            }
        }
        
    }
}
