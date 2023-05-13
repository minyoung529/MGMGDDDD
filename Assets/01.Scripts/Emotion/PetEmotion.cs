using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmotionType
{
    None,
    Smile,
    Hate,
    Angry,
}
public class PetEmotion : MonoBehaviour
{
    [SerializeField] Texture2D[] textures;

    private EmotionType emotionType = EmotionType.None;
    public EmotionType GetEmotionType { get { return emotionType; } }

    private Material material;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        SetMaterial();
    }

    private void SetMaterial()
    {
        if (meshRenderer)
        {
            material = meshRenderer.material;
        }
        else
        {
            material = GetComponent<MeshRenderer>()?.material;
        }
    }

    public void SetEmotion(EmotionType type)
    {
        emotionType = type;
        if (material == null)
        {
            SetMaterial();
        }
        material.mainTexture = textures[(int)type];
    }

    [ContextMenu("Test")]
    public void Test()
    {
        SetEmotion(EmotionType.Smile);
    }
}
