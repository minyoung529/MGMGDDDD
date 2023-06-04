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

    Sequence seq;

    #region Property
    public Action OnDestroyPlatform { get; set; }
    public int Index { get; set; } = -1;
    public Color Color { get; set; }

    public bool IsDestroyed => isDestroyed;
    #endregion

    private LinePuzzleController controller;

    private void Awake()
    {
        controller = FindObjectOfType<LinePuzzleController>();
        fire = GetComponentInChildren<Fire>();
    }

    public void Initialize(int colorIdx, ref Color[] colors, ref Color[] matColors)
    {
        if (colorIdx < 0)
        {
            Destroy(connectPoint);
        }
        else
        {
            connectPoint.SetActive(true);
            pointRenderer.material = Instantiate(pointRenderer.sharedMaterial);

            pointRenderer.material.SetColor("_EmissionColor", matColors[colorIdx]);
            pointRenderer.material.color = matColors[colorIdx];
            Color = colors[colorIdx];
            Index = colorIdx;
        }

        originalColor = boardRenderer.material.color;
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

        else if (other.CompareTag(Define.OIL_PET_TAG) && controller.SelectedPieces.Contains(this))
        {
            isLightOn = true;
            boardRenderer.material.DOColor(controller.SelectedPiece.Color, 1f);
        }

        else if (controller.IsPainting && !isBurning && other.name == "Trigger")
        {
            controller.InsertCurrentPiece(this);
        }
    }

    private void DestroyPlatform()
    {
        if (isBurning || isDestroyed) return;
        if (!gameObject.activeSelf) return;

        controller.BurningPieces.Add(this);
        isBurning = true;
        isDestroyed = true;

        seq = DOTween.Sequence();
        seq.AppendInterval(4f);
        seq.AppendCallback(Hide);
        seq.AppendCallback(() =>
        {
            destroyParticle.Play();
            OnDestroyPlatform?.Invoke();
            controller.BurningPieces.Remove(this);
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

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        isDestroyed = false;
        isBurning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.OIL_PET_TAG) && controller.SelectedPieces.Contains(this))
        {
            isLightOn = true;
            boardRenderer.material.DOColor(controller.SelectedPiece.Color, 1f);
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
        fire?.Burn();
    }
}
