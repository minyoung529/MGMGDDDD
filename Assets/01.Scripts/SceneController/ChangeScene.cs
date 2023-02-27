using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [Flags]
    public enum ChangeType
    {
        None = 0,
        OnCollisionEnter = 1,
        OnCollisionExit = 2,
    }

    [SerializeField] private SceneType sceneType;
    [SerializeField] private ChangeType changeType;
    [SerializeField] private LayerMask collideLayer;

    public void GoTo()
    {
        if (go) return;
        SceneController.ChangeScene(sceneType);
        go = true;
    }

    public bool IsRight(ChangeType type) => (changeType & type) != 0;

    private bool go = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsRight(ChangeType.OnCollisionEnter) && ((1 << collision.gameObject.layer) & collideLayer) != 0)
        {
            GoTo();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsRight(ChangeType.OnCollisionExit) && ((1 << collision.gameObject.layer) & collideLayer) != 0)
        {
            Debug.Log("dd");
            GoTo();
        }
    }
}

