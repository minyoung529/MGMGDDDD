using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HintEnum
{
    None = 0

    , OilRoom = 1 << 0
    , FireRoom = 1 << 1
    , StickyRoom = 1 << 2
    , Bridge = 1 << 3
}
public class HintLightController : MonoBehaviour
{
    private static Dictionary<HintEnum, ChangeEmission[]> lightsDictionary = new Dictionary<HintEnum, ChangeEmission[]>();
    static HintEnum curIndex = HintEnum.None;

    private void Awake()
    {
        ResetHintLight();
    }

    public static void Hint(HintEnum hint)
    {
        OffHint(hint);
        curIndex = hint;
        OnHint(hint);
    }

    #region Set

    private void ResetHintLight()
    {
        lightsDictionary.Clear();

        // 전부 다 꺼놓기
        AllOffLight();
    }

    public static void AddLights(HintEnum state, ChangeEmission[] emissions)
    {
        lightsDictionary.Add(state, emissions);
    }

    #endregion

    #region On/Off

    private static void OnHint(HintEnum hint)
    {
        foreach (ChangeEmission light in lightsDictionary[hint])
        {
            light.Change();
        }
    }
    private static void OffHint(HintEnum hint)
    {
        foreach (ChangeEmission light in lightsDictionary[hint])
        {
            light.BackToOriginalColor();
        }
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
