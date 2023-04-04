using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor = -1.1f;
    [SerializeField] private Color outlineColor = Color.blue;

    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
    }

    #region Set

    [ContextMenu("OnOutline")]
    public void OnOutline()
    {
        if (outlineRenderer != null)
            outlineRenderer.enabled = true;
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
        MeshFilter filter = outlineObject.AddComponent<MeshFilter>();

        MeshRenderer originMesh = gameObject.GetComponent<MeshRenderer>();
        MeshRenderer render = outlineObject.AddComponent<MeshRenderer>();

        filter.mesh = originFilter.mesh;

        render.material = outlineMat;
        render.material.SetColor("_OutlineColor", color);
        render.material.SetFloat("_Scale", scaleFactor);
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.transform.localScale = new Vector3(1f, 1f, 1f);
        render.enabled = false;

        render.name = gameObject.name + "_Outline";

        return render;
    }

    #endregion

}