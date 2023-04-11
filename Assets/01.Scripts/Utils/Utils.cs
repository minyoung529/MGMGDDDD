using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetOrAddComponent<T>(this Component com) where T : Component
    {
        if (!com.gameObject.TryGetComponent<T>(out var component))
        {
            component = com.gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        if (!obj.TryGetComponent<T>(out var component))
        {
            component = obj.AddComponent<T>();
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

    public static Vector3 MultiplyVec(this Vector3 thisVec, Vector3 vector)
    {
        return new Vector3(thisVec.x * vector.x, thisVec.y * vector.y, thisVec.z * vector.z);
    }

    public static int LayerToInteger(LayerMask layer)
    {
        int count = 0;
        while (layer > 1)
        {
            layer >>= 1;
            count++;
        }

        return count;
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