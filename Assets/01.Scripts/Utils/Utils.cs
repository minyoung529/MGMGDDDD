using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static void ChangeAllLayer(this Transform transform, int layer)
    {
        ChangeLayersRecursively(transform, layer);
    }

    public static void ChangeLayersRecursively(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;

        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, layer);
        }
    }
}
