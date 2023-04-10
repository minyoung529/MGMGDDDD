using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public PetFlag PetType => petType;

    private List<Renderer> outlineRenderer = new List<Renderer>();

    void Start()
    {
        InitRenderer();
    }

    private void InitRenderer()
    {
        Renderer originRender = transform.GetComponent<Renderer>();
        if (originRender != null)
        {
            outlineRenderer.Add(originRender);
            Renderer outline = CreateOutline(transform, outlineMaterial, outlineScaleFactor, Color.white);
        }


        for (int i = 0; i < transform.childCount; i++)
        {
            Renderer render = transform.GetChild(i).GetComponent<Renderer>();
            if (render == null) continue;

            outlineRenderer.Add(render);
            Renderer outline = CreateOutline(transform.GetChild(i).transform, outlineMaterial, outlineScaleFactor, Color.white);
            if (outline != null)
            {
                outlineRenderer[i] = outline;
            }
        }
    }

    #region Set

    [ContextMenu("OnOutline")]
    public void OnOutline()
    {
        if (outlineRenderer != null) SetEnableRenderer(true);
    }
    [ContextMenu("OffOutline")]
    public void OffOutline()
    {
        if (outlineRenderer != null) SetEnableRenderer(false);
    }

    private void SetEnableRenderer(bool value)
    {
        Debug.Log(value);
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
        GameObject outlineObject = Instantiate(new GameObject(), origin.position, origin.rotation, origin);

        MeshFilter originFilter = origin.GetComponent<MeshFilter>();
        ProBuilderMesh originProFilter = origin.GetComponent<ProBuilderMesh>();

        if(originProFilter == null && originFilter == null)
        {
            return null;
        }

        AddMesh(outlineObject, originFilter);

        MeshRenderer render = outlineObject.AddComponent<MeshRenderer>();
        render.material = outlineMat;
        render.material.SetColor("_OutLine_Color", color);
        render.material.SetFloat("_Outline_Thickness", scaleFactor);
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.transform.localScale = new Vector3(1f, 1f, 1f);
        render.enabled = false;

        render.name = gameObject.name + "_Outline";

        return render;
    }

    private void AddMesh(GameObject newObj, MeshFilter meshFilter)
    {
        MeshFilter filter = newObj.AddComponent<MeshFilter>();
        filter.mesh = meshFilter.mesh;
    }
    #endregion

}