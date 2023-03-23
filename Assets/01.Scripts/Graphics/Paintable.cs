using UnityEngine;

public class Paintable : MonoBehaviour
{
    const int TEXTURE_SIZE = 1024;
    public float extendsIslandOffset = 1;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;

    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture GetMask() => maskRenderTexture;
    public RenderTexture GetUVIslands() => uvIslandsRenderTexture;
    public RenderTexture GetExtend() => extendIslandsRenderTexture;
    public RenderTexture GetSupport() => supportTexture;
    public Renderer GetRenderer() => rend;

    [SerializeField]
    private int scale = 1;

    void Start()
    {
        maskRenderTexture = new RenderTexture(TEXTURE_SIZE * scale, TEXTURE_SIZE* scale, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE * scale, TEXTURE_SIZE * scale, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE * scale, TEXTURE_SIZE * scale, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture = new RenderTexture(TEXTURE_SIZE * scale, TEXTURE_SIZE * scale, 0);
        supportTexture.filterMode = FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);

        PaintManager.Instance.InitTextures(this);
    }

    void OnDisable()
    {
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}