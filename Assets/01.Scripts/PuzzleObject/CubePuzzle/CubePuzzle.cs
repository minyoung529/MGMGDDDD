using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class CubePuzzle : MonoBehaviour
{
    private Action OnSuccess;
    private bool isSuccess = false;
    [SerializeField] private Renderer bottomRenderer;

    private Rigidbody cubeRigid;

    private void Start()
    {
        OnSuccess += LightBottom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSuccess) return;

        if (other.CompareTag("CubePuzzle")&&
            other.transform.GetSiblingIndex() == transform.GetSiblingIndex())
        {
            OnSuccess?.Invoke();
            cubeRigid = other.attachedRigidbody;
            MoveCubeToCenter(other.transform);
            isSuccess = true;
        }
    }

    public void ResetPuzzle()
    {
        if (!isSuccess) return;

        isSuccess = false;
        bottomRenderer.material.SetColor("_EmissionColor", Color.black);
        cubeRigid.GetComponent<FallTest>().SavePoint();

        cubeRigid.constraints ^= RigidbodyConstraints.FreezePosition;
    }

    #region SUCCESS
    private void LightBottom()
    {
        bottomRenderer.material.SetColor("_EmissionColor", Color.cyan * 5f);
    }

    private void MoveCubeToCenter(Transform cube)
    {
        cubeRigid.constraints = RigidbodyConstraints.FreezeAll;

        Vector3 center = bottomRenderer.transform.position;
        center.y = bottomRenderer.transform.position.y;

        cube.DOMove(center, 0.5f);
        cube.DORotate(Vector3.up * -90f, 0.5f);
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

    public void ConnectCube(Transform cube)
    {
        cube.position += Vector3.up * 5f;
        cube.DOMoveY(cube.position.y - 5f, 1f).SetEase(Ease.InExpo).OnComplete(() => MoveCubeToCenter(cube));
    }
}
