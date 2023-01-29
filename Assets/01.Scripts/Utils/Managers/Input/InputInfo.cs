using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputInfo
{
    public InputInfo(InputType type, KeyCode code) {
        inputType = type;
        keyCode = code;
    }

    public InputType inputType;
    public KeyCode keyCode;
}
