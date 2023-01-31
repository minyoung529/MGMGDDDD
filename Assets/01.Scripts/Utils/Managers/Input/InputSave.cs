using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputSave
{
    public InputSave(InputAction action, InputInfo[] code) {
        this.action = action;
        this.code = code;
    }

    [SerializeField] private InputAction action;
    public InputAction Action => action;

    [SerializeField] private InputInfo[] code;
    public InputInfo[] Code => code;
}
