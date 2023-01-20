using UnityEngine;

[System.Serializable]
public struct InputCode
{
    public InputCode(KeyCode keyCode, bool scroll) {
        this.keyCode = keyCode;
        this.scroll = scroll;
    }

    public KeyCode keyCode;
    public bool scroll; //false : ScrollDown, true : ScrollUp
}
