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
    private float scale = 1;

    void Awake()
    {
        int newScale = (int)(TEXTURE_SIZE * scale);

        maskRenderTexture = new RenderTexture(newScale, newScale, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIslandsRenderTexture = new RenderTexture(newScale, newScale, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture = new RenderTexture(newScale, newScale, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture = new RenderTexture(newScale, newScale, 0);
        supportTexture.filterMode = FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);
    }

    void Start()
    {
        PaintManager.Instance.InitTextures(this);
    }

    void OnDisable()
    {
        if(maskRenderTexture!= null) maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}