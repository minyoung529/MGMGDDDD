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
    private ThirdPersonCameraControll cameraController;

    #region Property
    public int ConnectCount => connectCnt;
    public Rigidbody RopeRigid => playerRopeRigid;
    #endregion

    private void Start()
    {
        connectObject = gameObject.GetOrAddComponent<ConnectObject>();
        connectPet = gameObject.GetOrAddComponent<ConnectPet>();
        rigid = GetComponent<Rigidbody>();
        cameraController = GetComponent<ThirdPersonCameraControll>();

        playerRope = Instantiate(wirePrefab);
        playerRope.ConnectStartPoint(playerRopeRigid);

        SetInitState();
    }

    void Update()
    {
        if (cameraController.IsAim && Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnConnect();
        }
    }

    /// <summary>
    /// ������ �Ǿ��� �� ȣ��Ǵ� �ݹ��Լ�
    /// </summary>
    private void OnConnect(WireController wire)
    {
        cameraController.SetAim(false);

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
        Camera camera = GameManager.Instance.MainCam;
        Vector3 screenCenter = new Vector3(camera.pixelWidth * 0.5f, camera.pixelHeight * 0.5f);

        Ray ray = camera.ScreenPointToRay(screenCenter);

        Debug.DrawRay(camera.transform.position, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, conenctedLayer))
        {
            Debug.Log(hitInfo.transform.gameObject.name);
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
            if (connectCnt == 2) return;

            if (connectCnt == 1)
            {
                OnConnect(playerRope);
            }
            else
            {
                // ����Ǿ��ִ� �� 1 �̻��̸� �Ӹ��� �̵�
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
