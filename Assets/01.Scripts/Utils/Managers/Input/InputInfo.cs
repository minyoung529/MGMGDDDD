using UnityEngine;

[System.Serializable]
public struct InputInfo
{
    public InputInfo(InputType type, KeyCode code) {
        inputType = type;
        keyCode = code;
    }

    public InputType inputType;
    public KeyCode keyCode;
}
