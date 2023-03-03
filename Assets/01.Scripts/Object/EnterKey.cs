using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class EnterKey : MonoBehaviour
{
    [SerializeField] UnityEvent toggle;

    private bool isToggle = false;

    private void OnTriggerEnter(Collider other)
    {
        InputManager.StartListeningInput(InputAction.Interaction, Enter);
    }
    private void OnTriggerExit(Collider other)
    {
        InputManager.StopListeningInput(InputAction.Interaction, Enter);
    }

    private void Enter(InputAction input, float value)
    {
        if (isToggle) return;
        isToggle = true;
        toggle.Invoke();
    }

}
