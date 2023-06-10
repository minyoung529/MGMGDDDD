using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    Sprite sprite;

    [SerializeField]
    SpriteAtlas atlas;

    [SerializeField]
    private bool onAwake = false;

    void Awake()
    {
        image ??= GetComponent<Image>();

        if (onAwake)
        {
            ChangeSprite();
        }
    }

    public void ChangeSprite()
    {
        if (image == null || atlas == null) return;

        image.sprite = atlas.GetSprite(sprite.name);
    }

    public void ChangeSprite(SpriteAtlas atlas, Sprite sprite)
    {
        if (image == null) return;

        this.atlas = atlas;
        this.sprite = sprite;

        ChangeSprite();
    }

    void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (sprite == null && image != null)
            sprite = image.sprite;
    }
}
