using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static CursorLockMode cursorMode => Cursor.lockState;
    public static bool cursorVisible => Cursor.visible;
    
    public static void MouserCursorEdit(bool isEnable, CursorLockMode isLocked)
    {
        Cursor.lockState = isLocked;
        Cursor.visible = isEnable;
    }

    public static void EditCursorSprite(Texture2D tex)
    {
        Cursor.SetCursor(tex, Vector3.zero, CursorMode.ForceSoftware);
    }
}
