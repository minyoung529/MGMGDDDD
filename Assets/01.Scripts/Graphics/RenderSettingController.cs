using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class RenderSettingController
{
    private static Color ambientLight;
    private static float reflectionIntensity;
             
    private static bool fog;
    private static Color fogColor;
    private static float fogDensity;

    public static void Start()
    {
        StoreOriginalValue();
    }

    private static void StoreOriginalValue()
    {
        ambientLight = RenderSettings.ambientLight;
        reflectionIntensity = RenderSettings.reflectionIntensity;
        fog = RenderSettings.fog;
        fogColor = RenderSettings.fogColor;
        fogDensity = RenderSettings.fogDensity;
    }

    public static void OriginalRenderSetting(float duration)
    {
        SetAmbientLight(ambientLight, duration);
        SetReflectionIntensity(reflectionIntensity, duration);
        SetFogColor(fogColor, duration);
        SetFogDensity(fogDensity, duration);

        RenderSettings.fog = fog;
    }

    public static Color SetAmbientLight(Color ambiendLight, float duration)
    {
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, ambiendLight, duration);
        return RenderSettings.ambientLight;
    }

    public static float SetReflectionIntensity(float reflectionIntensity, float duration)
    {
        DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, reflectionIntensity, duration);
        return RenderSettings.reflectionIntensity;
    }

    public static Color SetFogColor(Color fogColor, float duration)
    {
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration);
        return RenderSettings.fogColor;
    }

    public static float SetFogDensity(float fogDensity, float duration)
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, fogDensity, duration);
        return RenderSettings.fogDensity;
    }

    public static bool Setfog(bool fog)
    {
        bool temp = RenderSettings.fog;
        RenderSettings.fog = fog;
        return temp;
    }
}
