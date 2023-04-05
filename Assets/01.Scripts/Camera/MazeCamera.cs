using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCamera : MonoBehaviour
{
    private float speed = 20f;

    private Dictionary<InputAction, Action<InputAction, float>> actionDict = new Dictionary<InputAction, Action<InputAction, float>>();

    private void Start()
    {
        StartListen();
    }

    private void OnDestroy()
    {
        StopListen();
    }

    #region Listen

    private void StartListen()
    {
        actionDict.Add(InputAction.Move_Forward, (action, value) => CameraMove(Vector3.up));
        actionDict.Add(InputAction.Back, (action, value) => CameraMove(Vector3.down));
        actionDict.Add(InputAction.Move_Left, (action, value) => CameraMove(Vector3.left));
        actionDict.Add(InputAction.Move_Right, (action, value) => CameraMove(Vector3.right));

        foreach (var kvp in actionDict)
            InputManager.StartListeningInput(kvp.Key, kvp.Value);
    }

    private void StopListen()
    {
        foreach (var kvp in actionDict)
            InputManager.StopListeningInput(kvp.Key, kvp.Value);
    }
    #endregion

    private void CameraMove(Vector3 dir)
    {
        transform.Translate(dir * Time.deltaTime * speed);
    }
}
