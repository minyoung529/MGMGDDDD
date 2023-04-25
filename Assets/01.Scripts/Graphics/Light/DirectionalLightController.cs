using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class DirectionalLightController : MonoBehaviour
{
    new private static Light light;
    private static Quaternion originalRotation;
    private static Color originalColor;

    private void Awake()
    {
        light = GetComponent<Light>();

        if (light.type != LightType.Directional)
            Destroy(gameObject);

        originalRotation = light.transform.rotation;
        originalColor = light.color;
    }

    public static void ChangeRotation(Quaternion rot, float duration = 0f)
    {
        light.transform.DORotateQuaternion(rot, duration);
    }

    public static void BackToOriginalRotation(float duration = 0f)
    {
        light.transform.DORotateQuaternion(originalRotation, duration);
    }

    public static void ChangeColor(Color color, float duration)
    {
        light.DOColor(color, duration);
    }

    public static void BackToOriginalColor(float duration = 1f)
    {
        light.DOColor(originalColor, duration);
    }

    public static void ChangeColor(string hexCode)
    {
        if (ColorUtility.TryParseHtmlString($"#{hexCode}", out Color color))
        {
            light.DOColor(color, 1f);
        }
    }
}
