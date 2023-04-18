using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFloating : MonoBehaviour
{
    [SerializeField] private bool loop = false;
    [SerializeField] private float strength = 2.5f;

    private void Awake()
    {
        if (loop) OnFluffy();
    }

    public void OnFluffy()
    {

    }

    public void OffFluffy()
    {
        
    }
}
