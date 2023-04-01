using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightController : MonoBehaviour
{
    new private static Light light;
    private static Quaternion originalRotation;

    private void Awake()
    {
        light = GetComponent<Light>();

        if (light.type != LightType.Directional)
            Destroy(gameObject);

        originalRotation = light.transform.rotation;
    }

    public static void ChangeRotation(Quaternion rot, float duration = 0f)
    {
        light.transform.DORotateQuaternion(rot, duration);
    }

    public static void BackToOriginalRotation(float duration = 0f)
    {
        light.transform.DORotateQuaternion(originalRotation, duration);
    }
}
