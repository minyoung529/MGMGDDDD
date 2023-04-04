using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;

    private Renderer outlineRenderer;

    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
    }

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

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(new GameObject(), transform.position, transform.rotation, transform);
        outlineObject.AddComponent<MeshFilter>();

        MeshRenderer originMesh = gameObject.GetComponent<MeshRenderer>();
        MeshRenderer render = outlineObject.AddComponent<MeshRenderer>();
        render = originMesh;

        render.transform.localScale = new Vector3(1f, 1f, 1f);

        render.material = outlineMat;
        render.material.SetColor("_OutlineColor", color);
        render.material.SetFloat("_Scale", scaleFactor);
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        render.enabled = false;

        return render;
    }

}