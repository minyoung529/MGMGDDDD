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

    public void OriginalRenderSetting(float duration)
    {
        SetAmbiendLight(ambientLight, duration);
        SetReflectionIntensity(reflectionIntensity, duration);
        SetFogColor(fogColor, duration);
        SetFogDensity(fogDensity, duration);

        RenderSettings.fog = fog;
    }

    public static void SetAmbiendLight(Color ambiendLight, float duration)
    {
        DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, ambiendLight, duration);
    }

    public static void SetReflectionIntensity(float reflectionIntensity, float duration)
    {
        DOTween.To(() => RenderSettings.reflectionIntensity, x => RenderSettings.reflectionIntensity = x, reflectionIntensity, duration);
    }

    public static void SetFogColor(Color fogColor, float duration)
    {
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, fogColor, duration);
    }

    public static void SetFogDensity(float fogDensity, float duration)
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, fogDensity, duration);
    }

    public static void Setfog(bool fog)
    {
        RenderSettings.fog = fog;
    }
}
