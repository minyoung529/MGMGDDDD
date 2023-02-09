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
        OnCollisionEnter = 0 << 1,
        OnCollisionExit = 1 << 1,
    }

    [SerializeField] private SceneType sceneType;
    [SerializeField] private ChangeType changeType;
    [SerializeField] private LayerMask collideLayer;

    public void GoTo() => SceneController.ChangeScnee(sceneType);

    public bool IsRight(ChangeType type) => (changeType & type) != 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsRight(ChangeType.OnCollisionEnter) && collision.gameObject.layer == collideLayer)
        {
            GoTo();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsRight(ChangeType.OnCollisionExit) && collision.gameObject.layer == collideLayer)
        {
            GoTo();
        }
    }
}

