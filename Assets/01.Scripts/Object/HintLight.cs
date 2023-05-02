using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class HintLight : MonoBehaviour
{
    [SerializeField] HintEnum state;
    [SerializeField] Color changeColor;

    private ChangeEmission[] lights;

    private void Awake()
    {
        lights = transform.GetComponentsInChildren<ChangeEmission>();
    }

    private void Start()
    {
        SettingColor();
        HintLightController.AddLights(state, lights);
    }

    [ContextMenu("Hint")]
    public void OnHint()
    {
        HintLightController.Hint(state);
    }

    private void SettingColor()
    {
        foreach(ChangeEmission changeEmission in lights)
        {
            changeEmission.SetColor(changeColor);
        }
    }
}
