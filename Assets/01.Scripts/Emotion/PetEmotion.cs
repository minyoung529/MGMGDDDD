using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmotionType
{
    None,
    Smile,
    Angry,
    Sad
}
public class PetEmotion : MonoBehaviour
{
    [SerializeField] Texture2D[] textures;

    Material material;

    private EmotionType emotionType = EmotionType.None;
    public EmotionType GetEmotionType { get { return emotionType; } }

    private void Awake()
    {
        material = GetComponent<MeshRenderer>()?.material;
    }

    public void SetEmotion(EmotionType type)
    {
        emotionType = type;
        material.mainTexture= textures[(int)type];
    }

    [ContextMenu("Test")]
    public void Test()
    {
        SetEmotion(EmotionType.Smile);
    }
}
