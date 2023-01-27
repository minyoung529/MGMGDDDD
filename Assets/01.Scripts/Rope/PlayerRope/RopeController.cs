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

    private void ConnectTarget(InputAction action = InputAction.TryConnect, InputType type = InputType.GetKeyDown, float val = 0f)
    {
        if (!ThirdPersonCameraControll.IsRopeAim) return;
        if (type != InputType.GetKeyDown) return;

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
        Debug.Log("FAlSE");
        playerRope.startRigid.isKinematic = playerRope.endRigid.isKinematic = false;
        playerRope.startJoint.autoConfigureConnectedAnchor = true;
        playerRope.Active(true);

        connectCnt = 0;
    }

    public void UnConnect(InputAction action = InputAction.UnConnect, InputType type = InputType.GetKeyDown, float val = 0f)
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
}
