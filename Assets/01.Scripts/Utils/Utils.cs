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

    public static Vector3 GetMid(Vector3 from, Vector3 to)
    {
        float dist = Vector3.Distance(from, to);
        return from + (to - from).normalized * 0.5f * dist; ;
    }
}


public struct Pair<T, Q>
{
    public T first;
    public Q second;

    public Pair(T first, Q second)
    {
        this.first = first;
        this.second = second;
    }
}