using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandlierListner : MonoBehaviour
{
    private Action<ChandlierListner> onLighting;
    private DetectOil oilDetector;

    public bool IsOilContact => oilDetector.IsContactOil;

    private void Awake()
    {
        oilDetector = GetComponent<DetectOil>();
    }

    public void ListeningOnLighting(Action<ChandlierListner> action)
    {
        onLighting += action;
    }

    public void Lighting()
    {
        onLighting?.Invoke(this);
    }
}
