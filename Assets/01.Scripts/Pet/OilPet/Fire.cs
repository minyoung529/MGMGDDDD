using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    bool isBurn = false;
    Material mat;



    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.color= Color.white;
        isBurn= false;
    }

    public void Burn()
    {
        isBurn = true;
        mat.color = Color.red;
    }

}
