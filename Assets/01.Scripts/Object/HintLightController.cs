using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HintEnum
{
    None = -1

    , OilRoom = 0
    , FireRoom = 1
    , StickyRoom = 2
    , Bridge = 3
}
public class HintLightController : MonoBehaviour
{
    private Dictionary<HintEnum, HintLight> lightsDictionary = new Dictionary<HintEnum, HintLight>();
    private HintEnum curState = HintEnum.None;

    void Awake()
    {
        HintLight[] lights = FindObjectsOfType<HintLight>();

        foreach (HintLight hintLight in lights)
        {
            try
            {
                lightsDictionary.Add(hintLight.State, hintLight);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public void Hint(HintEnum hint)
    {
        OffHint(curState);
        curState = hint;
        OnHint(curState);
    }

    public void Hint(int idx)
    {
        OffHint(curState);
        curState = (HintEnum)idx;
        OnHint(curState);
    }


    #region Set

    private void ResetHintLight()
    {
        AllOffLight();
    }

    #endregion

    #region On/Off

    public void OnHint(HintEnum hint)
    {
        if (hint == HintEnum.None) return;

        lightsDictionary[hint].HintOn();
    }

    public void OffHint(HintEnum hint)
    {
        if (hint == HintEnum.None) return;

        lightsDictionary[hint].HintOff();
    }
    #endregion

    #region All On/Off
    public void AllOffLight()
    {
        for (int i = 0; i < lightsDictionary.Count; i++)
        {
            OffHint((HintEnum)i);
        }
    }
    public void AllOnLight()
    {
        for (int i = 0; i < lightsDictionary.Count; i++)
        {
            OnHint((HintEnum)i);
        }
    }
    #endregion
}
