using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderSettings : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private bool isFirstState = false;

    [Header("VARIABLE")]
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

    [SerializeField]
    private bool[] changeVariable = new bool[5] { true, true, true, true, true };

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
                //if (transform.position.x > other.transform.position.x)
                //{
                //    if (!onceChange) return;
                //    Back();
                //}
                //else
                {
                    if (onceChange) return;

                    onceChange = true;
                    Change();
                }
            }
        }
    }

    public void TriggerFog(bool isTrigger)
    {
        fog = isTrigger;
        Change();
    }

    public void Change()
    {
        if (changeVariable[0])
            oReflectionIntensity = RenderSettingController.SetReflectionIntensity(reflectionIntensity, duration);
        if (changeVariable[1])
            oAmbientLight = RenderSettingController.SetAmbientLight(ambientLight, duration);
        if (changeVariable[2])
            oFog = RenderSettingController.Setfog(fog);
        if (changeVariable[3])
            oFogColor = RenderSettingController.SetFogColor(fogColor, duration);
        if (changeVariable[4])
            oFogDensity = RenderSettingController.SetFogDensity(fogDensity, duration);
    }

    public void Back()
    {
        if (changeVariable[0])
            RenderSettingController.SetReflectionIntensity(oReflectionIntensity, duration);
        if (changeVariable[1])
            RenderSettingController.SetAmbientLight(oAmbientLight, duration);
        if (changeVariable[2])
            RenderSettingController.Setfog(oFog);
        if (changeVariable[3])
            RenderSettingController.SetFogColor(oFogColor, duration);
        if (changeVariable[4])
            RenderSettingController.SetFogDensity(oFogDensity, duration);
    }
}
