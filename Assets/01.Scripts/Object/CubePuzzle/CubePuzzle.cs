using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CubePuzzle : MonoBehaviour
{
    private Action<int> OnSuccess;
    private bool isSuccess = false;
    public bool IsSuccess => isSuccess;
    private bool hasCube = false;
    [SerializeField] private Renderer bottomRenderer;

    #region  Installed Cube
    private CubeObject cubeObject;
    private int cubeIdx = 0;
    #endregion

    private readonly string CUBE_PUZZPE_TAG = "CubePuzzle";

    private int index = 0;

    [SerializeField] private UnityEvent<Transform> onPuttedCube;

    [SerializeField]
    private HotAirController hotAirController;

    private void Start()
    {
        index = transform.GetSiblingIndex();
        OnSuccess += LightBottom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSuccess || hasCube) return;
        if (!other.CompareTag(CUBE_PUZZPE_TAG)) return;

        ConnectCube(other);
    }

    private void ConnectCube(Collider other)
    {
        if (hasCube || isSuccess) return;

        cubeObject = other.GetComponent<CubeObject>();
        cubeObject.Installed();

        MoveCubeToCenter(other.transform, () => hasCube = true);

        cubeIdx= int.Parse(other.gameObject.name);

        if (cubeIdx == index)
        {
            isSuccess = true;
            OnSuccess?.Invoke(index + 1000);
        }
        else
        {
            OnSuccess?.Invoke(index);
        }

        onPuttedCube?.Invoke(other.transform);

        hotAirController.SetIndex(cubeIdx, index);
    }

    public void ResetPuzzle()
    {
        hasCube = false;
        isSuccess = false;
        bottomRenderer.material.SetColor("_EmissionColor", Color.white);
        cubeObject?.Respawn();
        cubeObject = null;
        hotAirController.OnReset(cubeIdx);
    }

    #region SUCCESS
    private void LightBottom(int v)
    {
        bottomRenderer.material.SetColor("_EmissionColor", Color.cyan * 5f);
        //StartCoroutine(Delay());
    }

    private void MoveCubeToCenter(Transform cube, Action onEnd)
    {
        Vector3 center = bottomRenderer.transform.position;
        center.y = bottomRenderer.transform.position.y;

        cube.DOMove(center, 0.5f).OnComplete(() => onEnd.Invoke());
        cube.DOLocalRotate(Vector3.up * 90f, 0.5f);
    }
    #endregion

    public void ListeningOnSuccess(Action<int> action, bool listen = true)
    {
        if (listen)
            OnSuccess += action;
        else
            OnSuccess -= action;
    }
}
