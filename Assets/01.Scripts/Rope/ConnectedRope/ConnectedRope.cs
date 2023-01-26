using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ConnectedRope : MonoBehaviour
{
    [SerializeField]
    private SlingShot slingShot;
    private LineRenderer line;

    new private BoxCollider collider;
    private Vector3 mid;

    #region Property
    public Vector3 Mid { get => mid; set => mid = value; }
    public Action OnConnect;
    #endregion

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (line?.positionCount >= 3)
        {
            line?.SetPosition(1, mid);
        }
    }

    public void Connect(Vector3 from, Vector3 to, LineRenderer lineRenderer)
    {
        line = lineRenderer;
        SetLine(from, to);
        SetCollider(from, to);

        transform.position = mid;
        transform.right = (to - from).normalized;

        OnConnect?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Define.PLAYER_LAYER)
        {
            if (Input.GetKey(KeyCode.S) && collision.transform.position.y < mid.y)
            {
                collider.isTrigger = true;
            }
        }
    }

    private void SetLine(Vector3 from, Vector3 to)
    {
        mid = Utils.GetMid(from, to);

        line.positionCount = 3;
        line.SetPosition(0, from);
        line.SetPosition(1, mid);
        line.SetPosition(2, to);
    }

    private void SetCollider(Vector3 from, Vector3 to)
    {
        Vector3 size;
        size.x = Vector3.Distance(from, to);
        size.y = size.z = line.startWidth;

        collider.size = size;
    }
}
