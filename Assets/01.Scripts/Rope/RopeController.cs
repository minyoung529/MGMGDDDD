using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;

public class RopeController : MonoBehaviour
{
    [SerializeField]
    private WireController wirePrefab;
    [SerializeField]
    private ConnectedRope conenctedRope;

    private List<WireController> petRopes = new List<WireController>();
    private List<ConnectedObject> pets = new List<ConnectedObject>();
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();

    [SerializeField]
    private Rigidbody petRopeRigid;
    [SerializeField]
    private Rigidbody playerRopeRigid;

    WireController playerRope;

    [SerializeField]
    private LayerMask conenctedLayer;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        playerRope = CreateRope(playerRopeRigid, false);
        conenctedRope = Instantiate(conenctedRope);
        conenctedRope.UnConnect();

        CreateRope(petRopeRigid);

        SetFirstPosition();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnConnect(0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }
    }

    private void ConnectTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.farClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = (mousePos - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, dir);

        // TEST
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, conenctedLayer))
        {
            playerRope.gameObject.layer = Define.ROPE_LAYER;
            TryConnect(hitInfo.transform, playerRope);
        }
    }

    /// <summary>
    /// 연결을 시도하는 함수
    /// </summary>
    private void TryConnect(Transform target, WireController wire)
    {
        ConnectedObject obj = target.GetComponent<ConnectedObject>();
        if (connectedObjs.Find(x => x == obj)) return;

        if (1 << target.gameObject.layer == Define.PET_LAYER)
        {
            if (connectedObjs.Count == 0)
            {
                wire.TryConnect(obj, OnConnect);
            }
        }
        else if (1 << target.gameObject.layer == Define.CONNECTED_OBJECT_LAYER)
        {
            // 연결되어있는 게 1 이상이면 머리가 이동
            wire.TryConnect(obj, OnConnect, connectedObjs.Count > 0);
        }
    }

    /// <summary>
    /// 연결이 되었을 때 호출되는 콜백함수
    /// </summary>
    private void OnConnect(ConnectedObject connectedObj, WireController wire)
    {
        if (1 << connectedObj.gameObject.layer == Define.PET_LAYER)
        {
            ConnectPet(connectedObj);
            connectedObj?.Connect(petRopes.Last(), false);
        }

        else if (1 << connectedObj.gameObject.layer == Define.CONNECTED_OBJECT_LAYER)
        {
            ConnectObject(connectedObj, wire);
        }
    }

    #region UnConnect
    private void UnConnect(int index)
    {
        while (connectedObjs.Count > index)
        {
            connectedObjs[index].UnConnect();
            connectedObjs.RemoveAt(index);
        }

        SetFirstPosition();
    }
    #endregion

    private WireController CreateRope(Rigidbody target = null, bool isAdd = true)
    {
        WireController wire = Instantiate(wirePrefab);

        if (target)
        {
            wire.ConnectStartPoint(target);
        }

        if (isAdd)
        {
            petRopes.Add(wire);
        }

        return wire;
    }

    private void SetFirstPosition()
    {
        playerRope.ConnectStartPoint(playerRopeRigid);
        playerRope.startRigid.isKinematic = playerRope.endRigid.isKinematic = false;
        playerRope.startJoint.autoConfigureConnectedAnchor = true;
        playerRope.Active(true);

        playerRope.transform.ChangeAllLayer(Define.ROPE_LAYER);
    }

    private void ConnectPet(ConnectedObject connectedObj)
    {
        if (pets.Find(x => x == connectedObj) == null)
        {
            if (pets.Count > 0)
            {
                CreateRope(pets.Last().Rigid);
            }

            pets.Add(connectedObj);
        }
    }

    private void ConnectObject(ConnectedObject connectedObj, WireController wire)
    {
        Rigidbody ropeRigid = wire.endRigid;

        if (connectedObjs.Count == 0)
        {
            playerRope.ConnectStartPoint(rigid);    // 몸에다 연결
            connectedObj.Connect(wire, false);      // 물체가 연결

            // TEST
            wire.startJoint.autoConfigureConnectedAnchor = false;
            wire.startJoint.connectedAnchor = Vector3.up * 0.6f;
        }
        else
        {
            ropeRigid = wire.startRigid;

            wire.ConnectStartPoint(connectedObj.Rigid); // 로프에 물체를 연결
            playerRope.Active(false);

            conenctedRope.Connect(connectedObjs[0].RopePosition, connectedObj.RopePosition);
        }


        ropeRigid.isKinematic = false;
        connectedObjs.Add(connectedObj);
    }
}
