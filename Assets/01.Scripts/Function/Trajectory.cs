using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private readonly int _lineSegmentCount = 20;
    private List<Vector3> _linePoints = new List<Vector3>();
    private Vector3 velocity = Vector3.zero;

    private void Start() {
        gameObject.SetActive(false);
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(Vector3 pos, Vector3 force, float mass) {
        gameObject.SetActive(true);
        transform.position = pos;
        velocity = force / mass;
    }

    public void StopDraw() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (!gameObject.activeSelf) return;
        float FlightDuration = 2 * velocity.y / Physics.gravity.y;
        float stepTime = FlightDuration / _lineSegmentCount;
        _linePoints.Clear();

        Vector3 beforePoint = Vector3.zero;
        int i = 0;
        for (; i < _lineSegmentCount; i++) {
            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                x: velocity.x * stepTimePassed,
                y: velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                z: velocity.z * stepTimePassed
                );
            if (i > 0) {
                if (Physics.Raycast(beforePoint, MovementVector - beforePoint, 1 << Define.BOTTOM_LAYER)) {
                    break;
                }
            }
            beforePoint = -MovementVector;
            _linePoints.Add(-MovementVector);
        }
        _lineRenderer.positionCount = i;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }
}
