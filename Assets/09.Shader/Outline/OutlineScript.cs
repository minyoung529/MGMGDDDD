using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

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
    [SerializeField] private float outlineScaleFactor = -1.1f;
    [SerializeField] private PetFlag petType;
    public PetFlag PetType => petType;

    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, Color.white);
    }

    #region Set

    [ContextMenu("OnOutline")]
    public void OnOutline()
    {
        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = true;
        }
    }
    [ContextMenu("OffOutline")]
    public void OffOutline()
    {
        if (outlineRenderer != null)
            outlineRenderer.enabled = false;
    }

    public void SetColor(Color color)
    {
        outlineRenderer.material.SetColor("_OutlineColor", color);
    }

    #endregion

    #region Draw_Outline

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(new GameObject(), transform.position, transform.rotation, transform);

        MeshFilter originFilter = gameObject.GetComponent<MeshFilter>();
        ProBuilderMesh originProFilter = gameObject.GetComponent<ProBuilderMesh>();

        if (originProFilter)
        {
            //AddProbuilderMesh(outlineObject, originProFilter);
            Debug.Log("PROBUILDER MESH!");
        }

        AddMesh(outlineObject, originFilter);

        MeshRenderer render = outlineObject.AddComponent<MeshRenderer>();
        render.material = outlineMat;
        render.material.SetColor("_OutlineColor", color);
        render.material.SetFloat("_Scale", scaleFactor);
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