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

    private Rigidbody cubeRigid;

    private readonly string CUBE_PUZZPE_TAG = "CubePuzzle";

    private int index = 0;

    [SerializeField] private UnityEvent<Transform> onPuttedCube;


    private void Start()
    {
        index = transform.GetSiblingIndex();
        OnSuccess += LightBottom;
    }

    private void Update()
    {
        if (hasCube && cubeRigid)
        {
            Vector3 cubePos = cubeRigid.position;
            cubePos.y = bottomRenderer.transform.position.y;

            if (Vector3.Distance(cubePos, bottomRenderer.transform.position) > 0.1f)
            {
                ResetPuzzle();
            }
        }
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

        cubeRigid = other.attachedRigidbody;
        MoveCubeToCenter(other.transform, () => hasCube = true);

        if (int.Parse(other.gameObject.name) == index)
        {
            isSuccess = true;
            OnSuccess?.Invoke(index + 1000);
        }
        else
        {
            OnSuccess?.Invoke(index);
        }

        onPuttedCube?.Invoke(other.transform);

        cubeRigid.GetComponent<Sticky>().OffSticky();
    }

    public void ResetPuzzle()
    {
        hasCube = false;
        isSuccess = false;
        bottomRenderer.material.SetColor("_EmissionColor", Color.white);
        cubeRigid = null;
    }

    #region SUCCESS
    private void LightBottom(int v)
    {
        bottomRenderer.material.SetColor("_EmissionColor", Color.cyan * 5f);
        //StartCoroutine(Delay());
    }

    private void MoveCubeToCenter(Transform cube, Action onEnd)
    {
        cubeRigid.constraints = RigidbodyConstraints.FreezeAll;

        Vector3 center = bottomRenderer.transform.position;
        center.y = bottomRenderer.transform.position.y;

        cube.DOMove(center, 0.5f).OnComplete(() => onEnd.Invoke());
        //cube.DORotate(Vector3.zero, 0.5f);
    }
    #endregion

    public void ListeningOnSuccess(Action<int> action, bool listen = true)
    {
        if (listen)
            OnSuccess += action;
        else
            OnSuccess -= action;
    }

    #region RESPAWN
    public void Respawn()
    {
        if (!cubeRigid) return;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        Transform cubeTransform = cubeRigid.transform;
        float targetY = cubeTransform.position.y - 4f;
        cubeTransform.GetComponent<Collider>().enabled = false;
        cubeRigid.constraints = RigidbodyConstraints.FreezeRotation;
        while (cubeTransform.position.y - targetY > 0.1f)
        {
            yield return null;
        }
        cubeTransform.GetComponent<RespawnObject>().Respawn();
        cubeRigid = null;
    }
    #endregion
}
