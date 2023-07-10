using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static ChangeScene;

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
    [SerializeField] private bool loading = true;
    [SerializeField] private UnityEvent OnChanged;

    [SerializeField]
    private bool isOnce = false;

    private int changeTime = 0;

    void Start()
    {
        SceneController.ListeningEnter(sceneType, OnChagneScene);
    }

    private void OnChagneScene()
    {
        isChanging = false;
    }

    [ContextMenu("Go To")]
    public void GoTo()
    {
        if (SceneController.IsChanging) return;

        if (isOnce && changeTime != 0)
            return;

        if (isChanging)
            return;

        isChanging = true;
        OnChanged?.Invoke();
        changeTime++;

        SceneController.ChangeScene(sceneType, loading);
    }

    public bool IsRight(ChangeType type) => (changeType & type) != 0;

    private bool isChanging = false;

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
            GoTo();
        }
    }

    void OnDestroy()
    {
        SceneController.StopListeningEnter(sceneType, OnChagneScene);
    }
}

