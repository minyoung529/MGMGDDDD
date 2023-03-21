using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformPiece : MonoBehaviour
{
    [Header("CONNECT_POINT")]
    [SerializeField]
    GameObject connectPoint;
    [SerializeField]
    private MeshRenderer pointRenderer;

    [Header("BOARD")]
    [SerializeField]
    private MeshRenderer boardRenderer;

    public int Index { get; set; } = -1;
    public Color Color { get; set; }
    private Color originalColor;

    private Fire fire;

    private bool isBurning = false;

    public Action OnDestroyPlatform { get; set; }

    private void Start()
    {
        NavMeshSurface navmeshSurface = GetComponent<NavMeshSurface>();
        fire = GetComponentInChildren<Fire>();
        navmeshSurface.RemoveData();
        navmeshSurface.BuildNavMesh();
    }

    public void Initialize(int c, ref Color[] colors)
    {
        if (c < 0)
        {
            Destroy(connectPoint);
        }
        else
        {
            connectPoint.SetActive(true);
            pointRenderer.material = Instantiate(pointRenderer.sharedMaterial);
            pointRenderer.material.color = Color = colors[c];
            Index = c;
        }

        originalColor = boardRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBurning && other.name == "Trigger")
        {
            LinePuzzleController.CurrentPiece = this;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            Fire fire = other.GetComponent<Fire>();

            if (fire && fire.IsBurn)
            {
                if (!isBurning)
                {
                    DestroyPlatform();
                }
            }
        }
    }

    private void DestroyPlatform()
    {
        isBurning = true;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(4f);
        seq.AppendCallback(() => gameObject.SetActive(false));
        seq.AppendCallback(() => isBurning = false);
        seq.AppendCallback(() => OnDestroyPlatform?.Invoke());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.OIL_PET_TAG))
        {
            boardRenderer.material.color = LinePuzzleController.CurrentPiece.Color;
        }
    }

    private void ResetPuzzle()
    {
        gameObject.SetActive(true);
        boardRenderer.material.color = originalColor;
    }

    public void ResetOilSpread()
    {
        boardRenderer.material.color = originalColor;
    }

    public void Burn()
    {
        fire?.Burn();
    }
}
