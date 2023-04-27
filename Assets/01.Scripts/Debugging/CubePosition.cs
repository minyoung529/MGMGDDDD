using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePosition : MonoBehaviour
{
    [SerializeField]
    private Color color = Color.red;

    [SerializeField]
    private bool isFill = true;

    [SerializeField]
    private bool isOutline = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
        Gizmos.matrix = rotationMatrix;

        if (isFill)
        {
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
        if (isOutline)
        {
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
