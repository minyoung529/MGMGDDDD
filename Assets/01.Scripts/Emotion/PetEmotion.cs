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
    
    private void Awake()
    {
        material = GetComponent<MeshRenderer>()?.sharedMaterial;
    }

    public void SetEmotion(EmotionType type)
    {
        emotionType = type;
        if(material == null)
        {
            material = GetComponent<MeshRenderer>().sharedMaterial;
        }
        material.mainTexture = textures[(int)type];
    }

    [ContextMenu("Test")]
    public void Test()
    {
        SetEmotion(EmotionType.Smile);
    }
}
