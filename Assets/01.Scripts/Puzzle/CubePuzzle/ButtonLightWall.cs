using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLightWall : MonoBehaviour
{
    [SerializeField] private UnityEvent toggleEvent;
    private ChangeEmission[] lights;

    private int count = 0;

    private void Awake()
    {
        lights = GetComponentsInChildren<ChangeEmission>();

        ResetLight();
    }

    private void ResetLight()
    {
        count = 0;
        foreach(ChangeEmission emission in lights)
        {
            emission.BackToOriginalColor();
        }
    }

    public void OnLight()
    {
        if (count == lights.Length) return;

        lights[count].Change();
        count++;
        if(count == lights.Length)
        {
            Debug.Log("ADS");
            toggleEvent?.Invoke();
        }
    }
}
