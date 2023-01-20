using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputSave
{
    public InputSave(InputAction action, InputCode code) {
        this.action = action;
        this.code = code;
    }

    [SerializeField] private InputAction action;
    public InputAction Action => action;
    [SerializeField] private InputCode code;
    public InputCode Code => code;
}
