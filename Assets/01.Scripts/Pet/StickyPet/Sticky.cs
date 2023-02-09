using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private bool isSticky = false;
    public bool IsSticky { get { return isSticky; } set { isSticky = value; } }

    public void SetSticky()
    {
        if(isSticky) { return; }

        isSticky = true;
    }
}
