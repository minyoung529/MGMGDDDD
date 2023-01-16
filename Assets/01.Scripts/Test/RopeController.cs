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

    private List<WireController> petRopes = new List<WireController>();
    private List<ConnectedObject> pets = new List<ConnectedObject>();
    private List<ConnectedObject> connectedObjs = new List<ConnectedObject>();

    [SerializeField]
    private Rigidbody petRopePos;
    [SerializeField]
    private Rigidbody playerRopePos;

    private float distance = 1000f;

    WireController playerRope;

    [SerializeField]
    private LayerMask conenctedLayer;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        playerRope = CreateRope(playerRopePos, false);
        CreateRope(petRopePos);

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
            TryConnect(hitInfo.transform, playerRope);
        }
    }

    private void TryConnect(Transform target, WireController wire)
    {
        ConnectedObject obj = target.GetComponent<ConnectedObject>();
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
            connectedObj?.Connect(petRopes.Last(), false);
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
            if (petRopes.Count > 1)
            {
                Destroy(petRopes[index + 1].gameObject);
                petRopes.RemoveAt(index + 1);
            }
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
        playerRope.ConnectStartPoint(playerRopePos);
        playerRope.startRigid.isKinematic = false;
        playerRope.endRigid.isKinematic = false;
    }

    private void ConnectPet(ConnectedObject connectedObj)
    {
        if (pets.Count > 0)
        {
            CreateRope(connectedObj.Rigid);
        }

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
            playerRope.ConnectStartPoint(rigid);
            connectedObj.Connect(wire, false);
            ropeRigid = wire.endRigid;
            ropeRigid.isKinematic = true;
        }
        else
        {
            ropeRigid = wire.startRigid;
            wire.startJoint.connectedBody = connectedObj.Rigid;
            ropeRigid.isKinematic = false;
        }

        ropeRigid.transform.position = (connectedObj.RopePosition.position);
    }
}
