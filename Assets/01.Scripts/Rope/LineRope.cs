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
    public List<Vector3> ropePositions { get; set; } = new List<Vector3>();

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
            if (ropePositions.Count > 2) DetectCollisionExits();
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
        ropePositions.Clear();

        handle = t1;
        target = t2;

        ropePositions.Add(target.position);
        ropePositions.Add(handle.position);
    }

    #region Rope Detect
    private void DetectCollisionEnter()
    {
        if (Physics.Linecast(handle.position, ropePositions[ropePositions.Count - 2], out RaycastHit hit, collMask))
        {
            AddPosToRope(hit.point);
        }
    }

    private void DetectCollisionExits()
    {
        if (!Physics.Linecast(handle.position, ropePositions[ropePositions.Count - 3], collMask))
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    private void AddPosToRope(Vector3 _pos)
    {
        ropePositions.Insert(Count - 2, _pos);
    }

    private void UpdateRopePositions()
    {
        ropePositions[0] = target.position;
        ropePositions[ropePositions.Count - 1] = handle.position;

        lineRenderer.positionCount = ropePositions.Count;
        lineRenderer.SetPositions(ropePositions.ToArray());
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
