using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    [SerializeField]
    private ParticleSystem destroyParticle;

    private Color originalColor;

    private Fire fire;
    private bool isBurning = false;
    private bool isLightOn = false;
    private bool isDestroyed = false;

    private MeshRenderer[] renderers;
    private NavMeshSurface navmeshSurface;

    Sequence seq;

    #region Property
    public Action OnDestroyPlatform { get; set; }
    public int Index { get; set; } = -1;
    public Color Color { get; set; }
    #endregion

    private LinePuzzleController controller;

    private void Awake()
    {
        navmeshSurface = GetComponent<NavMeshSurface>();
        controller = FindObjectOfType<LinePuzzleController>();
        fire = GetComponentInChildren<Fire>();
    }

    public void Initialize(int c, ref Color[] colors, ref Color[] matColors)
    {
        if (c < 0)
        {
            Destroy(connectPoint);
        }
        else
        {
            connectPoint.SetActive(true);
            pointRenderer.material = Instantiate(pointRenderer.sharedMaterial);

            pointRenderer.material.SetColor("_EmissionColor", matColors[c]);
            pointRenderer.material.color = matColors[c];
            Color = colors[c];
            Index = c;
        }

        originalColor = boardRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBurning && other.name == "Trigger")
        {
            if (isDestroyed)
            {
                controller.PauseOilPet(true);
            }
            else
            {
                controller.PauseOilPet(false);
                LinePuzzleController.CurrentPiece = this;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.OIL_BULLET_TAG))
        {
            Fire fire = other.GetComponent<Fire>();

            if (fire && fire.IsBurn)
            {
                if (!isBurning && isLightOn)
                {
                    DestroyPlatform();
                }
            }
        }

        else if (other.CompareTag(Define.OIL_PET_TAG))
        {
            isLightOn = true;
            boardRenderer.material.DOColor(LinePuzzleController.SelectedPiece.Color, 1f);
        }
    }

    private void DestroyPlatform()
    {
        if (isBurning || isDestroyed) return;
        if (!gameObject.activeSelf) return;

        isBurning = true;
        isDestroyed = true;

        seq = DOTween.Sequence();
        seq.AppendInterval(4f);
        seq.AppendCallback(Hide);
        seq.AppendCallback(() =>
        {
            destroyParticle.Play();
            OnDestroyPlatform?.Invoke();
        });
    }

    private void Hide()
    {
        renderers ??= transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        isBurning = false;
    }

    private void Show()
    {
        renderers ??= transform.GetComponentsInChildren<MeshRenderer>();
        navmeshSurface.BuildNavMesh();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        //gameObject.SetActive(true);
        isDestroyed = false;
        isBurning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.OIL_PET_TAG))
        {
            isLightOn = true;
            boardRenderer.material.DOColor(LinePuzzleController.SelectedPiece.Color, 1f);
        }
    }

    public void ResetPuzzle()
    {
        seq.Kill();

        Show();
        boardRenderer.material.DOColor(originalColor, 1f);
    }

    public void ResetOilSpread()
    {
        boardRenderer.material.DOColor(originalColor, 1f);
        isLightOn = false;
    }

    public void Burn()
    {
        if (isLightOn)
            fire?.Burn();
    }
}
