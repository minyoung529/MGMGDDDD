using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LineRope : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Vector3 this[int index]
    {
        get
        {
            if (index > lineRenderer.positionCount - 1) return Vector3.zero;

            return lineRenderer.GetPosition(index);
        }
        set
        {
            if (index > lineRenderer.positionCount - 1) return;

            lineRenderer.SetPosition(index, value);
        }
    }
    public int Count => lineRenderer.positionCount;

    private Transform handle, target;

    private LineState state = LineState.None;

    #region Detect
    [SerializeField] LayerMask collMask;
    public List<Vector3> positions { get; set; } = new List<Vector3>();

    bool isPhysicsUpdate = true;
    #endregion

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Count < 2) return;

        if (state == LineState.Transform)
        {
            this[0] = target.position;
            this[Count - 1] = handle.position;
        }

        if (isPhysicsUpdate)
        {
            UpdateRopePositions();
            SetFixedValue();

            DetectCollisionEnter();
            if (positions.Count > 2) DetectCollisionExits();
        }
    }

    public void Connect(params Vector3[] list)
    {
        state = LineState.Vector;

        lineRenderer.positionCount = list.Length;
        lineRenderer.SetPositions(list);
    }

    public void SetTarget(Transform t1, Transform t2)
    {
        lineRenderer ??= GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        state = LineState.Transform;
        positions.Clear();

        handle = t1;
        target = t2;

        positions.Add(target.position);
        positions.Add(handle.position);
    }

    #region Rope Detect
    private void DetectCollisionEnter()
    {
        List<Pair<int, Vector3>> hits = new();

        // 너무 비효율적인 코드....................... 일단 해보자..
        for (int i = 0; i < positions.Count - 1; i++)
        {
            if (Physics.Linecast(positions[i], positions[i + 1], out RaycastHit hit, collMask))
            {
                if (Vector3.Distance(hit.point, positions[i]) < 0.01f || Vector3.Distance(hit.point, positions[i + 1]) < 0.01f)
                    continue;

                hits.Add(new Pair<int, Vector3>(i, hit.point));
            }
        }

        for (int i = 0; i < hits.Count; i++)
        {
            positions.Insert(hits[i].first + i + 1, hits[i].second);
        }
    }

    private void DetectCollisionExits()
    {
        List<int> indexes = new();

        // 너무 비효율적인 코드....................... 일단 해보자..
        for (int i = 0; i < positions.Count - 2; i++)
        {
            if (!Physics.Linecast(positions[i], positions[i + 2], out RaycastHit hit, collMask))
            {
                indexes.Add(i);
            }
        }

        for (int i = 0; i < indexes.Count; i++)
        {
            positions.RemoveAt(indexes[i] - i);
        }
    }

    private void UpdateRopePositions()
    {
        positions[0] = target.position;
        positions[positions.Count - 1] = handle.position;

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void SetFixedValue()
    {
        this[0] = target.position;
        this[Count - 1] = handle.position;
    }

    #endregion
}

enum LineState
{
    None, Transform, Vector, Count
}
