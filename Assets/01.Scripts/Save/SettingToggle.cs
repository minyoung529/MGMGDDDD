using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingToggle : MonoBehaviour
{
    public void Toggle()
    {
        Setting.Instance.ToggleActive();
    }
}
