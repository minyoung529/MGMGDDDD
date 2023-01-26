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

    private int connectCnt = 0;
    private Rigidbody rigid;

    #region Property
    public int ConnectCount => connectCnt;
    public Rigidbody RopeRigid => playerRopeRigid;
    #endregion

    private void Start()
    {
        connectObject = gameObject.GetOrAddComponent<ConnectObject>();
        connectPet = gameObject.GetOrAddComponent<ConnectPet>();
        rigid = GetComponent<Rigidbody>();

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
            UnConnect();
        }
    }

    /// <summary>
    /// 연결이 되었을 때 호출되는 콜백함수
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
            playerRope.Active(false);
            connectCnt++;
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
    /// 연결을 시도하는 함수
    /// </summary>
    private void TryConnect()
    {
        if (1 << target.gameObject.layer == Define.PET_LAYER)
        {
            playerRope.TryConnect(OnConnect, hitPoint);
        }
        else
        {
            if (connectCnt == 2) return;

            if (connectCnt == 1)
            {
                OnConnect(playerRope);
            }
            else
            {
                // 연결되어있는 게 1 이상이면 머리가 이동
                playerRope.TryConnect(OnConnect, hitPoint/*, true*/);
            }
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

        connectCnt = 0;
    }

    public void UnConnect()
    {
        connectObject.UnConnect();
        connectPet.UnConnect();
        SetInitState();
    }
}
