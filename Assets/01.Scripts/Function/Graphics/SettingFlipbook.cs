using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingFlipbook : MonoBehaviour
{
    [SerializeField] private bool isChild = false;
    [SerializeField] private bool useChildIdx = false;
    [SerializeField] private int tile;
    new private Renderer renderer;
    private readonly int TILE_HASH = Shader.PropertyToID("_Tile");

    void Start()
    {
        renderer = GetComponent<Renderer>();
        UseChildIdx();
    }

    public void SetTile(int tile)
    {
        renderer.material.SetFloat(TILE_HASH, tile);
    }

    private void UseChildIdx()
    {
        if (useChildIdx)
        {
            if (isChild)
            {
                SetTile(transform.parent.GetSiblingIndex());
            }
            else
            {
                SetTile(transform.GetSiblingIndex());
            }
        }
    }
}
