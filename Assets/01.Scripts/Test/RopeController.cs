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

    private List<WireController> wires = new List<WireController>();
    private List<ConnectedObject> pets = new List<ConnectedObject>();
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();
    private ConnectedObject playerConnect;

    private float distance = 1000f;

    [SerializeField]
    private LayerMask conenctedLayer;

    void Start()
    {
        playerConnect = Utils.GetOrAddComponent<ConnectedObject>(gameObject);

        CreateRope(playerConnect.RopePosRigid);
        SetFirstPosition();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnConnect(0);
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)(int)KeyCode.Alpha1 + i))
            {
                UnConnect(i);
            }
        }
    }

    private void ConnectTarget()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.farClipPlane;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = (mousePos - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, conenctedLayer))
        {
            TryConnect(hitInfo.transform, wires.Last());
        }
    }

    private void TryConnect(Transform target, WireController wire)
    {
        ConnectedObject obj = target.GetComponent<ConnectedObject>();
        if (obj == playerConnect) return;
        if (connectedObjs.Find(x => x == obj)) return;

        if (1 << target.gameObject.layer == Define.PET_LAYER)
        {
            wire.TryConnect(obj, OnConnect);
        }
        else if (1 << target.gameObject.layer == Define.CONNECTED_OBJECT_LAYER)
        {
            // 연결되어있는 게 1 이상이면 머리가 이동
            wire.TryConnect(obj, OnConnect, connectedObjs.Count > 0);
        }
    }

    private void OnConnect(ConnectedObject connectedObj, WireController wire)
    {
        if (1 << connectedObj.gameObject.layer == Define.PET_LAYER)
        {
            ConnectPet(connectedObj);
            connectedObj?.Connect(wire, false);
        }

        else if (1 << connectedObj.gameObject.layer == Define.CONNECTED_OBJECT_LAYER)
        {
            ConnectObject(connectedObj, wire);
        }

        connectedObjs.Add(connectedObj);
    }

    #region UnConnect
    private void UnConnect(int index)
    {
        while (connectedObjs.Count > index)
        {
            connectedObjs[index].UnConnect();
            connectedObjs.RemoveAt(index);

            // 플레이어의 줄은 지우지 않음
            if (wires.Count > 1)
            {
                Destroy(wires[index + 1].gameObject);
                wires.RemoveAt(index + 1);
            }
        }

        SetFirstPosition();
    }
    #endregion

    private void CreateRope(Rigidbody target = null)
    {
        WireController wire = Instantiate(wirePrefab);

        if (target)
        {
            wire.ConnectStartPoint(target);
        }
        wires.Add(wire);
    }

    private void SetFirstPosition()
    {
        wires[0].ConnectStartPoint(playerConnect.RopePosRigid);
        wires[0].startRigid.isKinematic = false;
        wires[0].endRigid.isKinematic = false;
    }

    private void ConnectPet(ConnectedObject connectedObj)
    {
        CreateRope(connectedObj.Rigid);

        if (pets.Find(x => x == connectedObj) == null)
        {
            pets.Add(connectedObj);
        }
    }

    private void ConnectObject(ConnectedObject connectedObj, WireController wire)
    {
        Rigidbody ropeRigid;

        if (connectedObjs.Count == 0)
        {
            wires[0].ConnectStartPoint(playerConnect.Rigid);
            connectedObj.Connect(wire, false);
            ropeRigid = wire.endRigid;
            ropeRigid.isKinematic = true;
        }
        else
        {
            ropeRigid = wire.startRigid;
            ropeRigid.isKinematic = true;
        }

        ropeRigid.transform.position = (connectedObj.RopePosition.position);
    }
}
