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

    private FixedJoint joint;

    [SerializeField]
    private LayerMask conenctedLayer;

    void Start()
    {
        playerConnect = Utils.GetOrAddComponent<ConnectedObject>(gameObject);

        CreateRope(playerConnect.RopePosition.transform);
        wires[0].ConnectStartPoint(playerConnect.RopePosition.transform);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConnectTarget();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ResetRopes();
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

        wire.StartCoroutine(wire.TryConnect(obj, OnConnect));
    }

    private void OnConnect(ConnectedObject connectedObj, WireController wire)
    {
        if (1 << connectedObj.gameObject.layer == Define.PET_LAYER)
        {
            CreateRope(connectedObj/*.RopePosition*/.transform);

            if (pets.Find(x => x == connectedObj) == null)
            {
                pets.Add(connectedObj);
            }
        }
        else if (1 << connectedObj.gameObject.layer == Define.CONNECTED_OBJECT_LAYER)
        {
            joint = gameObject.AddComponent<FixedJoint>();
            //joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
            //joint.angularXMotion = joint.angularYMotion = joint.angularZMotion = ConfigurableJointMotion.Limited;

            joint.connectedBody = wires[0].firstSegmentConnectedBody.GetComponent<Rigidbody>();
            wires[0].ConnectStartPoint(transform);
        }


        connectedObjs.Add(connectedObj);
        connectedObj?.Connect(wire, false);
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

        if(joint)
        {
            Destroy(joint);
            wires[0].firstSegmentConnectedBody = wires[0].startRigid;
            wires[0].ConnectStartPoint(playerConnect.RopePosition.transform);
        }
    }

    private void ResetRopes()
    {
        UnConnect(0);
    }
    #endregion

    private void CreateRope(Transform target)
    {
        WireController wire = Instantiate(wirePrefab);
        wire.ConnectStartPoint(target);
        wires.Add(wire);
    }
}
