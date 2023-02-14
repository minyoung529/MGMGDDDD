using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackbarDirection : MonoBehaviour
{
    [SerializeField] private RectTransform rect = null;
    private bool isActive;
    [ContextMenu("test")]
    public void ReceiveSignal() {
        rect.offsetMax = new Vector2(200f, 200f);
    }
}