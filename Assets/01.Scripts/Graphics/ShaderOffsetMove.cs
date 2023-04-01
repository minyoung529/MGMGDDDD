using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderOffsetMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction = Vector2.down;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private string textureName = "_MainTex";
    private int offsetID;

    new private Renderer renderer;

    private void Start()
    {
        direction = direction.normalized;
        offsetID = Shader.PropertyToID(textureName);
        renderer = GetComponent<Renderer>();
    }

    public void Active() => isActive = true;

    public void Inactive() => isActive = false;

    private void Update()
    {
        if (!isActive) return;

        Vector2 curOffset = renderer.material.GetTextureOffset(offsetID);
        curOffset += Time.deltaTime * speed * direction;
        renderer.material.SetTextureOffset(offsetID, curOffset);
    }
}
