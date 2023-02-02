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
    public int ConnectCount { get => connectCnt; set => connectCnt = value; }
    public Rigidbody RopeRigid => playerRopeRigid;
    public WireController PlayerRope => playerRope;
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
        SetInput();
    }

    private void SetInput()
    {
        // ERROR
        //InputManager.StartListeningInput(InputAction.UnConnect, InputType.GetKeyDown, UnConnect);
        //InputManager.StartListeningInput(InputAction.TryConnect, InputType.GetKeyDown, ConnectTarget);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            UnConnect();
        if (Input.GetMouseButtonDown(0))
            ConnectTarget();
    }

    /// <summary>
    /// 연결이 되었을 때 호출되는 콜백함수
    /// </summary>
    private void OnConnect(WireController wire)
    {
        cameraController.SetRope();

        if (target.gameObject.layer == Define.PET_LAYER)
        {
            connectPet.Connect(target.GetComponent<ConnectedObject>());
        }
        else
        {
            connectObject.Connect(target.transform, hitPoint, wire);
            playerRope.Active(false);
            connectCnt++;
        }
    }

    private void ConnectTarget()
    {
        if (!ThirdPersonCameraControll.IsRopeAim) return;
        if (connectCnt >= 2) return;

        Camera camera = GameManager.Instance.MainCam;
        Vector3 screenCenter = new Vector3(camera.pixelWidth * 0.5f, camera.pixelHeight * 0.5f);

        if (Physics.Raycast(camera.ScreenPointToRay(screenCenter), out RaycastHit hitInfo, 1000f, conenctedLayer))
        {
            Debug.Log(hitInfo.transform.gameObject.name);
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
            if (connectCnt == 0)
            {
                playerRope.TryConnect(OnConnect, hitPoint);
            }
            else
            {
                OnConnect(playerRope);
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

        connectCnt = 0;
    }

    public void UnConnect()
    {
        connectObject.UnConnect();
        connectPet.UnConnect();
        SetInitState();

        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0f;

        transform.eulerAngles = eulerAngles;
    }

    private void OnDestroy()
    {
        //InputManager.StopListeningInput(InputAction.UnConnect, InputType.GetKeyDown, UnConnect);
        //InputManager.StopListeningInput(InputAction.TryConnect, InputType.GetKeyDown, ConnectTarget);
    }

    #region Debug
    private void OnDrawGizmos()
    {
        Color gizmoColor = Color.red;
        gizmoColor.a = 0.4f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(RopeRigid.position, Define.MAX_ROPE_DISTANCE);
    }
    #endregion
}
