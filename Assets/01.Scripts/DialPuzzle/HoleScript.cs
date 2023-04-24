using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour {
    [SerializeField] private float holeOriginRadius = 10f;
    [SerializeField] private float minRadius;
    public float MinRadius => minRadius;
    [SerializeField] private float maxRadius;
    public float MaxRadius => maxRadius;
    [SerializeField] private float radius = 0f;
    public float Radius {
        get {
            return radius;
        }
        set {
            if (value < MinRadius) {
                radius = MinRadius;
                return;
            }
            if (MaxRadius < value) {
                radius = MaxRadius;
                return;
            }
            radius = value;
        }
    }

    private List<int> vertexIndex = new List<int>();
    private List<Vector3> direction = new List<Vector3>();

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;

    private void Start() {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
        for (int i = 0; i < mesh.vertexCount; i++)
            if (mesh.vertices[i].sqrMagnitude <= Mathf.Pow(holeOriginRadius, 2)) {
                vertexIndex.Add(i);
                Vector3 dir = mesh.vertices[i];
                dir.y = 0;
                dir = dir.normalized;
                direction.Add(dir);
            }
    }

    private void Update() {
        SetHoleSize(radius);
    }

    private void SetHoleSize(float radius) {
        Vector3[] vertex = mesh.vertices;
        for (int i = 0; i < vertexIndex.Count; i++) {
            vertex[vertexIndex[i]] = (direction[i] * radius) + (Vector3.up * vertex[vertexIndex[i]].y);
        }
        mesh.vertices = vertex;
        meshCollider.sharedMesh = mesh;
    }
}