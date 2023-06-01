using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmotionType
{
    None,
    Smile,
    Hate,
    Angry,
    Dizzy,
    Afraid, 
    Sad,
    Amazing,
    Satisfied,
    Twinkle,
    Count
}
public class PetEmotion : MonoBehaviour
{
    private EmotionType emotionType = EmotionType.None;
    public EmotionType GetEmotionType { get { return emotionType; } }

    private Material material;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private int tileHash = Shader.PropertyToID("_Tile");

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
        material.SetFloat(tileHash, (int)type);

        if (material == null)
        {
            SetMaterial();
        }
    }
}
