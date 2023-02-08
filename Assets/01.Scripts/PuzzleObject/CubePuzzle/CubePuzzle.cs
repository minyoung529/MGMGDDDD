using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePuzzle : MonoBehaviour
{
    private Action OnSuccess;
    private bool isSuccess = false;
    [SerializeField] private Renderer bottomRenderer;

    private void Start()
    {
        OnSuccess += LightBottom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSuccess) return;

        if (other.CompareTag("CubePuzzle") &&
            other.transform.GetSiblingIndex() == transform.GetSiblingIndex())
        {
            OnSuccess?.Invoke();
            MoveCubeToCenter(other.transform);
            isSuccess = true;
        }
    }

    public void ResetPuzzle()
    {
        isSuccess = false;
    }

    #region SUCCESS
    private void LightBottom()
    {
        bottomRenderer.material.SetColor("_EmissionColor", Color.cyan * 5f);
    }

    private void MoveCubeToCenter(Transform cube)
    {
        Vector3 center = cube.transform.position;
        center.x = bottomRenderer.transform.position.x;
        center.z = bottomRenderer.transform.position.z;

        cube.DOMove(center, 0.5f);
    }
    #endregion
    public void ListeningOnSuccess(Action action, bool listen = true)
    {
        if (listen)
        {
            OnSuccess += action;
        }
        else
        {
            OnSuccess -= action;
        }
    }
}
