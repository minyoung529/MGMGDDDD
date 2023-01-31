using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fire : MonoBehaviour
{
    float burningTime = 3f;
    bool isBurn = false;
    bool isOtherBurn = false;

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
        isOtherBurn = true;
        StartCoroutine(BurnReadTime());
    }

    IEnumerator BurnReadTime()
    {
        yield return new WaitForSeconds(burningTime);

        if(isOtherBurn)
        {
            isBurn = true;
            mat.color = Color.red;
            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isBurn)
        {
            Fire f = other.GetComponent<Fire>();
            if (f != null)
            {
                if (f.isBurn) return;
                mat.color = Color.red;
                f.DeleteBurn();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Fire f = other.GetComponent<Fire>();
        if (f != null)
        { 
            if (isOtherBurn)
            {
                isOtherBurn = false;
                StopCoroutine(BurnReadTime());
            }
        }
    }
}
