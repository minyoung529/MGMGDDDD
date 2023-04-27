using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActivatePlatform : MonoBehaviour
{
    private Collider[] colliders;
    private Renderer[] renderers;

    private Color[] originalColor;

    private readonly int OPACITY_HASH = Shader.PropertyToID("_Opacity");

    [SerializeField]
    private bool exceptMyself = false;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        Inactive();
    }

    public void Activate()
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            if (!renderers[i].material.HasProperty(OPACITY_HASH))
            {
                continue;
            }

            renderers[i].material.SetFloat(OPACITY_HASH, 1f);
        }

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (exceptMyself && colliders[i].gameObject == gameObject) continue;

            colliders[i].enabled = true;
        }
    }

    public void Inactive()
    {
        for (int i = 0; i < renderers.Length; ++i)
        {
            if (!renderers[i].material.HasProperty(OPACITY_HASH))
            {
                continue;
            }

            renderers[i].material.SetFloat(OPACITY_HASH, 0.3f);
        }

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (exceptMyself && colliders[i].gameObject == gameObject) continue;
            colliders[i].enabled = false;
        }
    }
}