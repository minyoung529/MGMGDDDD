using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class HintLight : MonoBehaviour
{
    [SerializeField] HintEnum state;
    [SerializeField] Color changeColor;
    [SerializeField] List<ChangeEmission> lights;

    public HintEnum State => state;

    [ContextMenu("Hint")]
    public void HintOn()
    {
        SettingColor();
        foreach (ChangeEmission changeEmission in lights)
        {
            changeEmission.Change();
        }
    }

    public void HintOff()
    {
        foreach (ChangeEmission changeEmission in lights)
        {
            changeEmission.BackToOriginalColor();
        }
    }

    private void SettingColor()
    {
        foreach (ChangeEmission changeEmission in lights)
        {
            changeEmission.SetColor(changeColor);
        }
    }
}
