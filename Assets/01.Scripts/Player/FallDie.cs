using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Cinemachine;

public class FallDie : MonoBehaviour
{
    private Vector3 hitPoint;

    [Header("Prefab")]
    [SerializeField] private CanvasGroup dieCanvas;

    [Header("Fall Check")]
    [SerializeField] private LayerMask bottomLayer;
    [SerializeField] private float dieDistance = 10f;

    private GameObject[] respawnTransforms;
    private bool respawning = false;

    private float[] originalRadius = new float[3];

    private List<CinemachineVirtualCameraBase> cams;
    private List<Transform> followTargets = new();
    private List<Transform> parents = new();

    void Start()
    {
        respawnTransforms = GameObject.FindGameObjectsWithTag("SpawnPoint");
        dieCanvas = Instantiate(dieCanvas);
        hitPoint = transform.position;
    }

    void Update()
    {
        if (respawning) return;

        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, 0.5f, bottomLayer))
        {
            hitPoint = hitInfo.point;
        }
        else if (hitPoint.y - transform.position.y > dieDistance)
        {
            Die();
        }
    }

    private void Die()
    {
        respawning = true;

        FarCamera();
        dieCanvas.gameObject.SetActive(true);
        dieCanvas.DOFade(1f, 1f).OnComplete(Respawn);
    }

    public void Respawn()
    {
        IOrderedEnumerable<GameObject> sorted = respawnTransforms.OrderBy(x => Vector3.Distance(x.transform.position, hitPoint));
        transform.position = sorted.First().transform.position;

        hitPoint = transform.position;

        for (int i = 0; i < cams.Count; i++)
        {
            cams[i].Follow = followTargets[i];
            cams[i].transform.SetParent(parents[i]);
        }

        dieCanvas.alpha = 1f;
        dieCanvas.DOFade(0f, 2f).OnComplete(() =>
        {
            dieCanvas.gameObject.SetActive(false);
            respawning = false;
        });
    }

    private void FarCamera()
    {
        followTargets.Clear();
        parents.Clear();
        cams = CameraSwitcher.Cameras;

        foreach (var cam in cams)
        {
            followTargets.Add(cam.Follow);
            parents.Add(cam.transform.parent);
            cam.Follow = null;
            cam.transform.SetParent(null);
        }
    }
}
