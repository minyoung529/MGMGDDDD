using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCamera : MonoBehaviour
{

    private float speed = 20f;

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
        InputManager.StartListeningInput(InputAction.Move_Forward, (action, value) => CameraMove(action, Vector3.up));
        InputManager.StartListeningInput(InputAction.Back, (action, value) => CameraMove(action, Vector3.down));
        InputManager.StartListeningInput(InputAction.Move_Left, (action, value) => CameraMove(action, Vector3.left));
        InputManager.StartListeningInput(InputAction.Move_Right, (action, value) => CameraMove(action, Vector3.right));
    }
    
    
    private void StopListen()
    {
        InputManager.StopListeningInput(InputAction.Move_Forward, (action, value) => CameraMove(action, Vector3.up));
        InputManager.StopListeningInput(InputAction.Back, (action, value) => CameraMove(action, Vector3.down));
        InputManager.StopListeningInput(InputAction.Move_Left, (action, value) => CameraMove(action, Vector3.left));
        InputManager.StopListeningInput(InputAction.Move_Right, (action, value) => CameraMove(action, Vector3.right));
    }
    

    #endregion

    private void CameraMove(InputAction input, Vector3 dir)
    {
        transform.Translate(dir * Time.deltaTime * speed);
    }
}
