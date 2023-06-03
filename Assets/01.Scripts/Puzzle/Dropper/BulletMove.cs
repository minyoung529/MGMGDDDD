using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private TileMoveController[] tiles;

    private void Awake()
    {
        tiles = transform.GetComponentsInChildren<TileMoveController>();
    }

    private void Start()
    {
        StartMove();
    }

    public void StartMove()
    {
        foreach(TileMoveController tile in tiles)
        {
            tile.SetActivate();
        }
    }

    public void StopMove()
    {
        foreach (TileMoveController tile in tiles)
        {
            tile.SetInactivate();
        }
    }
}
