using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTileTexture : MonoBehaviour
{
    [SerializeField] private int tile;
    private int tileHash = Shader.PropertyToID("_Tile");
    private Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetFloat(tileHash, (float)tile);
    }

    public void UpdateTexture(int type) {
        material.SetFloat(tileHash, type);
    }
}