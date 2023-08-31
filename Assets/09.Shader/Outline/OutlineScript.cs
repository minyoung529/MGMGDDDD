using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[Flags]
public enum PetFlag
{
    None = 0

    , OilPet = 1 << 0
    , FirePet = 1 << 1
    , StickyPet = 1 << 2
}

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private float outlineScaleFactor = 0.06f;
    [SerializeField] private PetFlag petType;
    [SerializeField] private UnityEvent onInteractPet;
    public PetFlag PetType => petType;

    [SerializeField] private string guideName;
    public string GuideName => guideName;

    [SerializeField]
    private List<GameObject> ignoreRenderers;

    private bool used = false;
    public bool IsInteract => used;

    [SerializeField]
    private bool isPetInteract = true;

    public bool IsPetInteract => isPetInteract;

    [SerializeField]
    private Color outlineColor;

    public Color OutlineColor => outlineColor;

    private Outline outline;

    [SerializeField]
    private bool useOldOutline = false;

    [SerializeField]
    private UnityEvent<Renderer[]> onInitialized;

    [SerializeField]
    private UnityEvent<Color> onColorChange;

    [SerializeField]
    private UnityEvent onEnterCursor;

    [SerializeField]
    private UnityEvent onExitCursor;

    void Start()
    {
        InitRenderer();
    }

    private void InitRenderer()
    {
        Renderer[] temp = transform.GetComponentsInChildren<MeshRenderer>();
        List<Renderer> renderers = new();

        foreach (Renderer renderer in temp)
        {
            if (renderer.GetType() == typeof(ParticleSystemRenderer))
            {
                continue;
            }

            if (ignoreRenderers.Contains(renderer.gameObject))
            {
                continue;
            }

            renderers.Add(renderer);
        }

        onInitialized?.Invoke(renderers.ToArray());

        if (useOldOutline)
        {
            return;
        }
        outline = GetComponent<Outline>();

        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 0f;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
        }

        outline.SetRenderer(renderers.ToArray());
    }

    #region Set

    [ContextMenu("OnOutline")]
    public void OnOutline()
    {
        Debug.Log("ONOUTLINE");

        if (outline != null)
        {
            SetEnableRenderer(true);
        }

        onEnterCursor?.Invoke();
    }
    [ContextMenu("OffOutline")]
    public void OffOutline()
    {
        Debug.Log("OFFOUTLINE");
        if (outline != null)
        {
            SetEnableRenderer(false);
        }

        onExitCursor?.Invoke();
    }

    private void SetEnableRenderer(bool value)
    {
        if (value)
        {
            onEnterCursor?.Invoke();
            outline.OutlineWidth = 10f;
        }
        else
        {
            onExitCursor?.Invoke();
            outline.OutlineWidth = 0f;
        }
    }

    public void SetColor(Color color)
    {
        if (outline != null)
            outline.OutlineColor = color;

        onColorChange?.Invoke(color);
    }

    #endregion

    public void OnInteract()
    {
        if (used) return;
        used = true;

        SelectedObject.CurInteractObject = this;
        OffOutline();
        onInteractPet?.Invoke();
    }
}