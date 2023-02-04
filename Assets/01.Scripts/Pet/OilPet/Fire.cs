using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fire : MonoBehaviour
{
    bool isBurn = false;
    float burningTime = 3f;
    
    Material mat;

    public bool IsBurn { get { return isBurn; } set { isBurn = value; } }

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        isBurn = false;
    }

    public void Burn()
    {
        isBurn = true;
        mat.color = Color.red;
    }

    public void DeleteBurn()
    {
        Burn();
        Destroy(gameObject);
    }

  
}
