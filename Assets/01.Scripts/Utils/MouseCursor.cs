using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static CursorLockMode cursorMode => Cursor.lockState;
    public static bool cursorVisible => Cursor.visible;
    
    public static void MouseCursorEdit(bool isEnable, CursorLockMode isLocked)
    {
        Cursor.lockState = isLocked;
        Cursor.visible = isEnable;
    }

    private void Awake()
    {
        MouseCursorEdit(false, cursorMode);
    }
}
