using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    [SerializeField] private bool canMove = false;
    private bool isSticky = false;
    public bool IsSticky { get { return isSticky; } set { isSticky = value; } }
    public bool CanMove { get { return canMove; } }

    public void SetSticky()
    {
        if(isSticky) return;

        isSticky = true;
    }
    
    public void NotSticky()
    {
        if(!isSticky) return;

        isSticky = false;
    }
}
