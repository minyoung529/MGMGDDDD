using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;
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
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor = 0.06f;
    [SerializeField] private PetFlag petType;
    [SerializeField] private UnityEvent onInteractPet;
    public PetFlag PetType => petType;

    private List<Renderer> outlineRenderer = new List<Renderer>();

    private int outlineLayer;
    private bool isInteract  = false;
    public bool IsInteract => isInteract;
    void Start()
    {
        outlineLayer = Utils.LayerToInteger( LayerMask.GetMask("Outline"));

        InitRenderer();
    }

    private void InitRenderer()
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Renderer newRenderer = CreateOutline(renderer.transform, outlineMaterial, outlineScaleFactor, Color.white);
            outlineRenderer.Add(newRenderer);
        }
    }

    #region Set

    [ContextMenu("OnOutline")]
    public void OnOutline()
    {
        if (isInteract) return;
        if (outlineRenderer != null) SetEnableRenderer(true);
    }
    [ContextMenu("OffOutline")]
    public void OffOutline()
    {
        if (outlineRenderer != null) SetEnableRenderer(false);
    }

    private void SetEnableRenderer(bool value)
    {
        for (int i = 0; i < outlineRenderer.Count; i++)
        {
            outlineRenderer[i].enabled = value;
        }
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < outlineRenderer.Count; i++)
        {
            outlineRenderer[i].material.SetColor("_OutLine_Color", color);
        }
    }

    #endregion

    #region Draw_Outline

    Renderer CreateOutline(Transform origin, Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = new GameObject($"{origin.name}_Outline");

        outlineObject.layer = outlineLayer;
        // Reset Transform
        outlineObject.transform.SetParent(origin);
        outlineObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        outlineObject.transform.localScale = new Vector3(1f, 1f, 1f);

        MeshFilter originFilter = origin.GetComponent<MeshFilter>();

        if (originFilter == null)
        {
            return null;
        }

        AddMesh(outlineObject, originFilter);

        MeshRenderer render = outlineObject.AddComponent<MeshRenderer>();
        render.material = outlineMat;
        render.material.SetColor("_OutLine_Color", color);
        render.material.SetFloat("_Outline_Thickness", scaleFactor);
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        render.enabled = false;

        return render;
    }

    private void AddMesh(GameObject newObj, MeshFilter meshFilter)
    {
        MeshFilter filter = newObj.AddComponent<MeshFilter>();
        filter.mesh = meshFilter.mesh;
    }
    #endregion

    public void OnInteract(Action act= null)
    {
        if (isInteract) return;
        isInteract = true;

        OffOutline();
        onInteractPet?.Invoke();
        if(act != null) act();
    }
}