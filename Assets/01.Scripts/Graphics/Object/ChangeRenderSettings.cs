using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChangeRenderSettings : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private bool isFirstState = false;

    [SerializeField]
    private Color ambientLight;
    [SerializeField]
    private float reflectionIntensity;

    [SerializeField]
    private bool fog;
    [SerializeField]
    private Color fogColor;
    [SerializeField]
    private float fogDensity;

    private Color oAmbientLight;
    private float oReflectionIntensity;
    private bool oFog;
    private Color oFogColor;
    private float oFogDensity;

    private bool onceChange = false;

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            if (isFirstState)
            {
                RenderSettingController.OriginalRenderSetting(duration);
            }
            else
            {
                if (transform.position.x > other.transform.position.x)
                {
                    if (!onceChange) return;
                    Back();
                }
                else
                {
                    onceChange = true;
                    Change();
                }
            }
        }
    }

    private void Change()
    {
        oReflectionIntensity = RenderSettingController.SetReflectionIntensity(reflectionIntensity, duration);
        oAmbientLight = RenderSettingController.SetAmbientLight(ambientLight, duration);
        oFog = RenderSettingController.Setfog(fog);
        oFogColor = RenderSettingController.SetFogColor(fogColor, duration);
        oFogDensity = RenderSettingController.SetFogDensity(fogDensity, duration);
    }

    private void Back()
    {
        RenderSettingController.SetReflectionIntensity(oReflectionIntensity, duration);
        RenderSettingController.SetAmbientLight(oAmbientLight, duration);
        RenderSettingController.Setfog(oFog);
        RenderSettingController.SetFogColor(oFogColor, duration);
        RenderSettingController.SetFogDensity(oFogDensity, duration);
    }
}
