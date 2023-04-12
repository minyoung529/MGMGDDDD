using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour {
    [Header("Ground")]
    [SerializeField] private PolygonCollider2D ground2DCollider;
    [SerializeField] private int angleAmount = 20;
    [SerializeField] private int radius = 25;

    public PolygonCollider2D hole2DCollider;
    public MeshCollider GeneratedMeshCollider;
    private Mesh GeneratedMesh;

    private void Start() {
        Vector2[] pos = new Vector2[angleAmount];
        pos[0] = Vector2.up * radius;
        float angle = 360 / angleAmount;
        for (int i = 1; i < angleAmount; i++)
            pos[i] = Quaternion.AngleAxis(angle, Vector3.forward) * pos[i - 1];
        ground2DCollider.points = pos;
        SetCollider();
    }

    private void FixedUpdate() {
        if (transform.hasChanged) {
            transform.hasChanged = false;
            SetCollider();
        }
    }

    private void SetCollider() {
        hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
        hole2DCollider.transform.localScale = transform.localScale;

        MakeHole2D();
        Make3DMeshCollider();
    }

    private void MakeHole2D() {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++) {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }
        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DMeshCollider() {
        if (GeneratedMesh != null) Destroy(GeneratedMesh);
        GeneratedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }
}
