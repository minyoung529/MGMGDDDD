using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
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
}
