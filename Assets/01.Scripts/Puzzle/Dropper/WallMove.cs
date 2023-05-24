using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    private TileMoveController[] tileMoves;

    void Awake()
    {
        tileMoves = new TileMoveController[transform.childCount - 1];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            tileMoves[i] = transform.GetChild(i).GetComponent<TileMoveController>();
        }
    }

    [ContextMenu("MOVE WALL")]
    public void MoveWall()
    {
        foreach (TileMoveController moveController in tileMoves)
        {
            moveController.SetActivate();
        }
    }

    public void StopWall()
    {
        foreach (TileMoveController moveController in tileMoves)
        {
            moveController.SetInactivate();
        }
    }
}
