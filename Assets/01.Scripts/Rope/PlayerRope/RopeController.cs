using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField]
    private WireController wirePrefab;

    [SerializeField]
    private Rigidbody playerRopeRigid;

    [SerializeField]
    private LayerMask conenctedLayer;

    WireController playerRope;

    private ConnectObject connectObject;
    private ConnectPet connectPet;

    // ---- Hit Variable ----
    private Vector3 hitPoint;
    private Transform target;

    private void Start()
    {
        connectObject = Utils.GetOrAddComponent<ConnectObject>(gameObject);
        connectPet = Utils.GetOrAddComponent<ConnectPet>(gameObject);

        playerRope = Instantiate(wirePrefab);
        playerRope.ConnectStartPoint(playerRopeRigid);

        SetInitState();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }

        if (Input.GetMouseButtonDown(1))
        {
            connectObject.UnConnect();
            connectPet.UnConnect();
        }
    }

    /// <summary>
    /// ������ �Ǿ��� �� ȣ��Ǵ� �ݹ��Լ�
    /// </summary>
    private void OnConnect(WireController wire)
    {
        if (1 << target.gameObject.layer == Define.PET_LAYER)
        {
            connectPet.Connect(target.GetComponent<ConnectedObject>());
        }
        else
        {
            connectObject.Connect(target.GetComponent<ConnectedObject>(), hitPoint, wire);
        }
    }

    private void ConnectTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.farClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = (mousePos - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, conenctedLayer))
        {
            hitPoint = hitInfo.point;
            target = hitInfo.transform;

            TryConnect();
        }
    }

    /// <summary>
    /// ������ �õ��ϴ� �Լ�
    /// </summary>
    private void TryConnect()
    {
        if (1 << target.gameObject.layer == Define.PET_LAYER)
        {
            playerRope.TryConnect(OnConnect, hitPoint);
        }
        else
        {
            // ����Ǿ��ִ� �� 1 �̻��̸� �Ӹ��� �̵�
            playerRope.TryConnect(OnConnect, hitPoint/*, true*/);
        }
    }

    /// <summary>
    /// Reset Rope to first
    /// </summary>
    private void SetInitState()
    {
        playerRope.ConnectStartPoint(playerRopeRigid);
        playerRope.startRigid.isKinematic = playerRope.endRigid.isKinematic = false;
        playerRope.startJoint.autoConfigureConnectedAnchor = true;
        playerRope.Active(true);
    }
}
